(function disableFareCalculatorFormHandler() {
    $('#Form-FareCalculate').submit(function () {
        console.log('Form submission prevented.');
        return false;
    });
})();

(function enableBookingRequestAjax() {
    $('#_FareCalculator #booking-RequestBooking').click(function () {
        $('#Form-FareCalculate .status .errors').css('display', 'none');
        $('#Form-FareCalculate .status .success').css('display', 'none');

        var params = {};
        params['Id'] = 0;
        params['Name'] = $('#Name').val().trim();
        params['Email'] = $('#Email').val().trim();
        params['Phone'] = $('#Phone').val().trim();
        params['LocationId'] = $('#LocationId').val().trim();
        params['SubLocationId'] = $('#SubLocationId').val().trim();
        params['RequestDate'] = $('#RequestDate').val().trim();
        params['ReturnDate'] = $('#ReturnDate').val().trim();
        params['VehicleId'] = $('#VehicleId').val().trim();
        params['WithDriver'] = $('#WithDriver').val().trim();
        params['PaymentMethodId'] = $('#PaymentMethodId').val().trim();
        params['RequestInitiated'] = $('#RequestInitiated').val().trim();
        params['RequestStatusId'] = $('#RequestStatusId').val().trim();
        params['__RequestVerificationToken'] = $('#Form-FareCalculate input[name=__RequestVerificationToken]').val().trim();

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
            url: '/visitor/CreateBookingRequest',
            data: params,
            type: 'POST',
            success: function (data) {
                $('#Form-FareCalculate .status .errors').css('display', 'none');
                $('#Form-FareCalculate .status .success').css('display', 'none');
                console.log('Ajax succeeded. Data received: ');
                console.log(data);
                if (data['status'] === 'success') {
                    $('#Form-FareCalculate .status .success')
                        .css('display', 'block')
                        .html('Your request has been forwarded. Order no. <u>' + data['data']['bookingRequestId'] + '</u>');
                    $('#Form-FareCalculate :input[data-resettable]').val(' ');
                } else {
                    if (data['exceptionOccurred'] === true) {
                        var html = '';
                        html += '<li>' + data['exceptionMessage'] + '</li>';
                        if (data['data']) {
                            for(let error of data['data']) {
                                html += '<li>' + error + '</li>';
                            }
                        }
                        $('#Form-FareCalculate .status .errors')
                            .css('display', 'block')
                            .find('ul')
                            .html(html);
                    } else {
                        $('#Form-FareCalculate .status .errors')
                            .css('display', 'block')
                            .find('ul')
                            .html(data['data']);
                    }
                    ///
                }
            },
            error: function (xhr) {
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