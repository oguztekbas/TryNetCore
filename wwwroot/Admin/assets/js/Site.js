$(document).on('keypress', '.only-number', function (event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        event.preventDefault();
        return false;
    }
    else { return true; }
});
$(document).on('keypress', '.currency', function (event) {
    var charCode = (event.which) ? event.which : event.keyCode
    var input = $(this).val();
    var commaIndex = input.indexOf(',');
    if (charCode < 48 || charCode > 57) {
        if (charCode == 44 && parseFloat(commaIndex) == -1) {
            // if (charCode == 44 && dotIndex != -1) { event.preventDefault(); }
            return true;
        }
        else {
            event.preventDefault();
            return false;
        }
    }
    else {
        console.log("commaIndex: " + commaIndex);
        if (commaIndex != -1) {
            var inputlist = input.split(",");
            if (inputlist.length > 1) {
                var rightside = inputlist[1];
                if (rightside.length > 3) {
                    event.preventDefault();
                    return false;
                }
            }
        }
        return true;
    }
});