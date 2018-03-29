(function handleGalleryMeta() {
    $('#image-meta-data button[data-btn-vehiclemeta-book]').click(function () {
        window.history.back();
        $('#Form-FareCalculate #vehicleCategory').val($(this).attr('data-category'));
        $('#Form-FareCalculate #vehicleCategory').change();
        var btn = this;
        setTimeout(function () {
            $('#Form-FareCalculate #VehicleId').val($(btn).attr('data-vehicle'));
        }, 3000);
        $('#_FareCalculator .status .success').css('display', 'block').html('Please fill out this form to continue...');
        $('#bs-example-navbar-collapse-1 [href=#_FareCalculator]').click();
        $('#Form-FareCalculate #Name').focus();
    });
})();