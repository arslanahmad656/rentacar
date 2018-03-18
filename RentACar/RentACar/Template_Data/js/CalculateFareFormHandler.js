(function disableFareCalculatorFormSubmission() {
    $('#Form-FareCalculate').submit(function () {
        console.log('Form submission prevented.');
        return false;
    });
})();
(function removeAllCarsOnFirstLoad() {
    $('#Form-FareCalculate #VehicleId').html('<option selected value>Select a category first</option>');
})();

(function selectCarsInCategory() {
    function loadCars() {
        var categoryId = Number($('#Form-FareCalculate #vehicleCategory').val().trim());
        $.ajax({
            url: '/visitor/GetVehiclesInCategory',
            data: {
                'categoryId': categoryId
            },
            type: 'GET',
            success: function (data) {
                console.log(data);
                if (data['exceptionOccurred'] === false) {
                    if (data['vehicleCount'] !== 0) {
                        let html = '';
                        let first = true;
                        let vehicles = data['vehicles'];
                        for (let vehicle of data['vehicles']) {
                            if (first) {
                                html += '<option selected value="' + vehicle['id'] + '">' + vehicle['title'] + '</option>';
                                first = false;
                            } else {
                                html += '<option value="' + vehicle['id'] + '">' + vehicle['title'] + '</option>';
                            }
                        }
                        $('#Form-FareCalculate #VehicleId').html(html);
                    } else {
                        $('#Form-FareCalculate #VehicleId').html('<option selected value>No vehicles found</option>');
                    }
                } else {
                    console.log('There was an error at server side during fetching the vehicles for the category.');
                }
            }
        });
    }

    $('#Form-FareCalculate #vehicleCategory').change(loadCars);
    loadCars(); // run this one time so that at first page load the vehicles are selected in the default selected category
})();

(function getFareEstimate() {
    $('#Form-FareCalculate #fareEstimateForm-SubmitAjax').click(function () {
        $('#Form-FareCalculate .status .errors').css('display', 'none');
        $('#Form-FareCalculate .status .success').css('display', 'none');

        var params = {};
        params['requestDate'] = $('#Form-FareCalculate #RequestDate').val().trim();
        params['returnDate'] = $('#Form-FareCalculate #ReturnDate').val().trim();
        params['vehicleId'] = Number($('#Form-FareCalculate #VehicleId').val().trim());
        params['withDriver'] = $('#Form-FareCalculate #WithDriver').val().trim() === "1" ? true : false;

        console.log('Following parameters are received: ');
        console.log(params);

        for (let prop in params) {
            if (params[prop] === '') {
                $('#Form-FareCalculate .status .errors')
                    .css('display', 'block')
                    .find('ul')
                    .html('<li>Please fill the ' + prop + ' field</li>');
                return false;
            }
        }

        $.ajax({
            url: '/visitor/GetFareEstimate',
            data: params,
            type: 'GET',
            success: function (data) {
                $('#Form-FareCalculate .status .errors').css('display', 'none');
                $('#Form-FareCalculate .status .success').css('display', 'none');
                console.log('Ajax succeeded. Data received: ');
                console.log(data);
                if (data['status'] === 'success') {
                    $('#_FareCalculator .status span[data-days]').html(data['noOfDays']);
                    $('#_FareCalculator .status span[data-price]').html(data['fare']);
                } else {
                    $('#_FareCalculator .status span[data-days]').html('0');
                    $('#_FareCalculator .status span[data-price]').html('0.00');
                    $('#Form-FareCalculate .status .success').css('display', 'none');
                    if (data['exceptionOccurred'] === true) {
                        let errors = data['exceptionMessage'];
                        console.log('Errors formulated: ' + errors);
                        $('#Form-FareCalculate .status .errors')
                            .css('display', 'block')
                            .find('ul')
                            .html(errors);
                    }
                }
            },
            error: function (xhr) {
                $('#_FareCalculator .status span[data-days]').html('0');
                $('#_FareCalculator .status span[data-price]').html('0.00');
                $('#Form-FareCalculate .status .success').css('display', 'none');
                console.log('Ajax error');
                console.log(xhr);
                $('#Form-FareCalculate .status .errors')
                            .css('display', 'block')
                            .find('ul')
                            .html('<li>' + 'Server error while calculating fare' + '<li>');
            }
        });
    });
})();