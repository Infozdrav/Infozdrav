// Polyfill
if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (search, pos) {
        return this.substr(!pos || pos < 0 ? 0 : +pos, search.length) === search;
    };
}

// Barcode scanner
var barcodeResultCallback = null;
function HandleBarcodeResult(data) {
    var result = {
        "raw": data
    }
    if (data.startsWith("R1")) { // Siemens
        result.type = "Siemens";

        // date
        var startOfDate = data.indexOf("?") + 1;
        var date = data.substring(startOfDate, startOfDate + 8);
        result.date = new Date(date.substring(0, 4), date.substring(4, 6) - 1, date.substring(6, 8));

        // lot 
        var startOfLot = data.indexOf("#") + 1;
        result.lot = data.substring(startOfLot, startOfDate - 1);
    }
    else if (data.charCodeAt(0) === 29 &&
        data.indexOf("FN") !== -1) { // TotalPSA
        result.type = "TotalPSA";

        // date
        var date = data.substring(19, 19 + 6);
        result.date = new Date(date.substring(0, 2) * 1 + 2000, date.substring(2, 4) - 1, date.substring(4, 6));

        result.lot = data.substring(27, 27 + 9); // lot
        result.gtin = data.substring(3, 3 + 14); // gtin
    }
    else if (data.charCodeAt(0) === 29) { // Roche
        result.type = "Roche";

        // date
        var date = data.substring(30, 30 + 6); 
        result.date = new Date(date.substring(0, 2)*1 + 2000, date.substring(2, 4) - 1, date.substring(4, 6));

        result.lot = data.substring(19, 19 + 8); // lot
        result.gtin = data.substring(3, 3 + 15); // gtin
    }

    if (barcodeResultCallback !== null)
        barcodeResultCallback(result);
}

function scanBarcode(callbackName) {
    barcodeResultCallback = window[callbackName];
    var scanner = window.open("/Scanner/Camera",
        "_blank",
        "height=480,width=640,toolbar=no,status=no,scrollbars=no,menubar=no,location=no,toolbar=no");
}


$(document).ready(function() {
    $(".barcode-scanner").click(function() {
        scanBarcode($(this).attr("data-callback"));
    });

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
})