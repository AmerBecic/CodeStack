$(function () {

    $('.datepicker').datepicker(
        {
            dateFormat: 'yy-mm-dd',
            minDate: new Date(),
            maxDate: AddSubtractMonths(new Date(), 4)
        }
    );

    function AddSubtractMonths(date, numMonths) //Can be moved to another js file for example: 'DateFunctions.js'
    {
        var month = date.getMonth();

        var milliSeconds = new Date(date).setMonth(month + numMonths);

        return new Date(milliSeconds);
    }

});