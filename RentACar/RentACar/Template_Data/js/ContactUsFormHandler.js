(function prepareContactUsSection() {

    // Disable form submission
    (function disableFormSubmission() {
        $('#Form-ContactUs').submit(function () {
            console.log('Contact us form prevented from being submitted.');
            return false;
        });
    })();

    // Enable form submission over ajax
    (function enableFormSubmissionAjax() {
        $('#contactUsForm-SubmitAjax').click(function () {
            $('#Form-ContactUs .status .errors').css('display', 'none');
            var params = {};
            params['Name'] = $('#Name').val().trim();
            params['Email'] = $('#Email').val().trim();
            params['Phone'] = $('#Phone').val().trim();
            params['Message'] = $('#Message').val().trim();
            params['__RequestVerificationToken'] = $('#Form-ContactUs input[name=__RequestVerificationToken]').val().trim();
            params['DatePosted'] = $('#DatePosted').val();
            params['Status'] = $('#Status').val();

            console.log(params);

            for(let prop in params) {
                if (params[prop] == '') {
                    $('#Form-ContactUs .status .errors')
                        .css('display', 'block')
                        .find('ul')
                        .html('<li>Please fill the ' + prop + ' field</li>');
                    return false;
                }
            }

            $.ajax({
                //url: '@Url.Action("CreateVisitorQuery")',
                url: '/visitor/CreateVisitorQuery',
                data: params,
                type: "POST",
                success: function (data) {
                    console.log('Ajax succeeded. Data:');
                    console.log(data);
                    if (data['status'] === 'success') {
                        $('#Form-ContactUs .status .errors').css('display', 'none');
                        $('#Form-ContactUs .status .success')
                            .css('display', 'block')
                            .html('Your message has been sent. Send another one.');
                        $('#Form-ContactUs :input[data-resettable]').val(' ');

                    } else {
                        $('#Form-ContactUs .status .success').css('display', 'none');
                        if (data['exceptionOccurred'] === true) {
                            let errors = '';
                            for(let error of data['data']) {
                                errors += '<li>' + error + '</li>';
                            }
                            console.log('Errors formulated: ' + errors);
                            $('#Form-ContactUs .status .errors')
                                .css('display', 'block')
                                .find('ul')
                                .html(errors);
                        } else {
                            $('#Form-ContactUs .status .errors')
                                .css('display', 'block')
                                .find('ul')
                                .html('<li>' + data['data'] + '<li>');
                        }
                    }
                },
                error: function (xhr) {
                    $('#Form-ContactUs .status .success').css('display', 'none');
                    console.log('Ajax error');
                    console.log(xhr);
                    $('#Form-ContactUs .status .errors')
                                .css('display', 'block')
                                .find('ul')
                                .html('<li>' + 'Server error while sending message' + '<li>');
                }
            });
        });
    })();

})();