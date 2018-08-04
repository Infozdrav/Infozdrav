using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Labena;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Infozdrav.Web.Helpers;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using Microsoft.WindowsAzure.Storage.File;

namespace Infozdrav.Web.Controllers
{
    public class ObdelavaVzorcevController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ObdelavaVzorcevController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IzberiVzorec()
        {
            ViewBag.DataSource = _dbContext.Samples.Include(m => m.NewId);
            return View();
        }

        [HttpPost]
        public IActionResult IzberiVzorec(ObdelavaVzorcevViewModel obdelava)
        {
            TempData.Put("vzorci", obdelava.Samples);
            return RedirectToAction("Obdelava");
        }

        public IActionResult Obdelava()
        {
//            ViewBag.DataSource = _dbContext.Samples;
            
//            ViewBag.DataSource = TempData.Get<List<string>>("vzorci");
//            var vzorci = TempData.Get<ObdelavaVzorcevViewModel>("vzorci");
            ViewBag.Types = _dbContext.SampleTypes.Select(m => m.SampleTypeName);
            return View(new ObdelavaVzorcevViewModel{Samples = TempData.Get<List<string>>("vzorci")});
        }

        [HttpPost]
        public IActionResult Obdelava(ObdelavaVzorcevViewModel obdelava)
        {
//            obdelava.Samples = TempData.Get<List<string>>("vzorci");
//            Console.WriteLine(obdelava.Samples == null);
//            Console.WriteLine(TempData.Get<List<string>>("vzorci"));
            
            
            
            var process = new Processing
            {
                Aparature = obdelava.Aparatura,
                Chemicals = obdelava.Kemikalije,
                Date = obdelava.DatumObdelave ?? DateTime.Today,
                IsolateName = _dbContext.SampleTypes.FirstOrDefault(m => m.SampleTypeName == obdelava.Izolat),
                Protocole = obdelava.Protokol
            };

            var samples = new List<Sample>();

            foreach (var sampleName in obdelava.Samples)
            {
                Console.WriteLine(sampleName);
                var sample = _dbContext.Samples.Include(m => m.NewId)
                    .First(m => m.NewId.ToString() == sampleName);
                var obdelano = new Sample
                {
                    Type = process.IsolateName,
                    Accepted = sample.Accepted,
                    ProjectName = sample.ProjectName,
                    SubscriberName = sample.SubscriberName,
                    NewId = new SampleId
                    {
                        SequenceNumber = sample.NewId.SequenceNumber,
                        AliquotSequenceNumber = sample.NewId.AliquotSequenceNumber,
                        IsolateType = sample.NewId.IsolateType,
                        ProcessedAliquotSequenceNumber = 0,
                        ProcessedIsolateType = process.IsolateName.ShortName,
                        Year = sample.NewId.Year
                    }
                };
                samples.Add(obdelano);
            }

            process.Samples = samples;

            _dbContext.Add(process);
            _dbContext.SaveChanges();

            switch (obdelava.Izolat)
            {
                case "DNA":
                    return RedirectToAction("RezultatiDna");
                case "RNA":
                    return RedirectToAction("RezultatiRna");
                case "cfDNA":
                    return RedirectToAction("RezultatiCfDna");
                case "serum":
                    return RedirectToAction("RezultatiSerum");
                case "plazma":
                    return RedirectToAction("RezultatiPlazma");
                case "protein":
                    return RedirectToAction("RezultatiProteini");
                case "celice (kultura)":
                case "celice (tkivo)":
                    return RedirectToAction("RezultatiCelice");
            }

            return RedirectToAction("Obdelava");
        }

        public IActionResult RezultatiProteini()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.Type.ShortName == "PRT");
            return View();
        }

        [HttpPost]
        public IActionResult RezultatiProteini(ProteinResultsViewModel results)
        {
            var result = new ProteinResults
            {
                SampleUsed = _dbContext.Samples.FirstOrDefault(m => m.NewId.ToString() == results.Sample),
                Concentration = results.Concentration,
                Volume = results.Volume
            };

            _dbContext.Add(result);
            _dbContext.SaveChanges();
            
            return Index();
        }

        public IActionResult RezultatiDna()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.Type.ShortName == "DNA");
            return View();
        }

        [HttpPost]
        public IActionResult RezultatiDna(DnaResultsViewModel results)
        {
            var result = new DnaResults
            {
                SampleUsed = _dbContext.Samples.FirstOrDefault(m => m.NewId.ToString() == results.Sample),
                Concentration = results.Concentration,
                Volume = results.Volume,
                A260A28 = results.A260A280,
                A260A230 = results.A260A230,
                Mass = results.Concentration * results.Volume
            };
            _dbContext.Add(result);
            _dbContext.SaveChanges();
            return Index();
        }

        public IActionResult RezultatiRna()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.Type.ShortName == "RNA");
            return View();
        }

        [HttpPost]
        public IActionResult RezultatiRna(RnaResultsViewModel results)
        {
            var result = new RnaResults
            {
                SampleUsed = _dbContext.Samples.FirstOrDefault(m => m.NewId.ToString() == results.Sample),
                Concentration = results.Concentration,
                Volume = results.Volume,
                A260A28 = results.A260A280,
                A260A230 = results.A260A230,
                Mass = results.Concentration * results.Volume,
                Rin = results.Rin
            };
            _dbContext.Add(result);
            _dbContext.SaveChanges();
            return Index();
        }


        public IActionResult RezultatiCelice()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.Type.ShortName == "CLC" || m.Type.ShortName == "CLT");
            return View();
        }

        [HttpPost]
        public IActionResult RezultatiCelice(CellResultsViewModel results)
        {
            var result = new CellResults
            {
                SampleUsed = _dbContext.Samples.FirstOrDefault(m => m.NewId.ToString() == results.Sample),
                CellNumber = results.CellNumber
            };
            _dbContext.Add(result);
            _dbContext.SaveChanges();
            return Index();
        }

        public IActionResult RezultatiSerum()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.Type.ShortName == "SBL");
            return View();
        }

        [HttpPost]
        public IActionResult RezultatiSerum(SerumPlasmaResultsViewModel results)
        {
            var result = new SerumPlasmaResults
            {
                SampleUsed = _dbContext.Samples.FirstOrDefault(m => m.NewId.ToString() == results.Sample),
                Volume = results.Volume
            };
            _dbContext.Add(result);
            _dbContext.SaveChanges();
            return Index();
        }

        public IActionResult RezultatiPlazma()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.Type.ShortName == "PBL");
            return View();
        }

        [HttpPost]
        public IActionResult RezultatiPlazma(SerumPlasmaResultsViewModel results)
        {
            var result = new SerumPlasmaResults
            {
                SampleUsed = _dbContext.Samples.FirstOrDefault(m => m.NewId.ToString() == results.Sample),
                Volume = results.Volume
            };
            _dbContext.Add(result);
            _dbContext.SaveChanges();
            return Index();
        }

        public IActionResult RezultatiCfDna()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.Type.ShortName == "cfD");
            return View();
        }

        [HttpPost]
        public IActionResult RezultatiCfDna(CfDnaResultsViewModel results)
        {
            var result = new CfDnaResults
            {
                SampleUsed = _dbContext.Samples.FirstOrDefault(m => m.NewId.ToString() == results.Sample),
                Volume = results.Volume,
                NumberOfCopies = results.NumberOfCopies,
                Concentration = results.NumberOfCopies * 3.3 / (1000 * results.Volume)
            };
            _dbContext.Add(result);
            _dbContext.SaveChanges();
            return Index();
        }

        public IActionResult Rezultati()
        {
            return View();
        }

        public IActionResult VzorciVObdelavi()
        {
            ViewBag.DataSource = _dbContext.Processings;
            return View();
        }

        public IActionResult PregledPodatkov()
        {
            return View();
        }

        public IActionResult DodajanjeVzorca()
        {
            return new SprejemVzorcevController(_dbContext).DodajanjeVzorca();
//            ViewBag.Fridges = _dbContext.Fridges.Select(m => m.Name);
//            ViewBag.Supplier = _dbContext.Acceptances.Select(m => m.SubscriberName.Name);
//            ViewBag.SupplierDates = _dbContext.Acceptances.Select(m => m.Date);
//            ViewBag.Boxes = _dbContext.Boxes.Select(m => m.BoxName);
//            ViewBag.Types = _dbContext.SampleTypes.Select(m => m.SampleTypeName);
//            ViewBag.Projects = _dbContext.Projects.Select(m => m.Name);
//            return View();
        }
    }
}