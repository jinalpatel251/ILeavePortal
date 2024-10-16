// Function for getting the Data Based upon Employee ID
function GetbyId(Id) {
    $('#id').css('border-color', 'lightgrey');
    $('#Name').css('border-color', 'lightgrey');
    $('#UserGmailId').css('border-color', 'lightgrey');
    $('#Password').css('border-color', 'lightgrey');
    $('#Confirmpassword').css('border-color', 'lightgrey');
    $('#Role').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Login/GetbyId/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function(result) {
            $('#Id').val(result.id);
            $('#Name').val(result.name);
            $('#EmailId').val(result.userEmailId);
            $('#Password').val(result.password);
            $('#confirmpassword').val(result.userEmailId);
            $('#', Role, ").val(result.Role);\",",

                $('#btnUpdate').show());
            $('#btnAdd').hide();
        },
        error: function(errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
