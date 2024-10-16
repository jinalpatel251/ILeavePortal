document.addEventListener('DOMContentLoaded', function () {
    LoadEmployeedetails();

});

function Add() {

    var loginObj = {
        UserEmailId: $('#UserEmailId').val(),
        Password: $('#Password').val(),
    }
    alert(loginObj);
    console.log(loginObj);
    $.ajax({
        url: "/Login/Add",
        data: { login: loginObj },
        type: "POST",

        dataType: "json",
        success: function (result) {
            if (loginObj.UserEmailId === "admin@gmail.com") {
                window.location.href = '/Login/Index';
            } else {
                window.location.href = '/Employee/Index';
            }
            loadData();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Cancel() {
    $.ajax({
        url: "/Login/Index",
        data: {},
        type: "POST",
        success: function (result) {
            window.location.href = '/Login/Index';

            $('#myModal').modal('hide');
            $('#loginlist').modal('show');

            //loadData();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function LoadEmployeedetails() {
    $.ajax({
        type: "POST",
        url: "/Admin/LoadEmployee",
        data: {},
        success: function (Obj) {
            if (!Obj.isSuccess) {
                $('._CustomMessage').text(Obj.message);
                $('#divAddEmployee').show();
                $('#divEmployeeIndex').hide();

            } else {
                LoadEmployeedata(Obj.list);
                $('#divAddEmployee').hide();
            }
        },
        error: function () {
            console.error("An error occurred while loading employee details.");
        }
    });
}

function LoadEmployeedata(List) {
    List.sort(function (a, b) {
        return b.id - a.id;
    });

    $('#tblemployee').DataTable({
        "pageLength": 10,
        "processing": true,
        "destroy": true,
        "data": List,
        "order": [[0, "DESC"]],
        "columns": [
            { "data": "id", "title": "Id", "width": "70px" },
            { "data": "name", "title": "Name", "width": "70px" },
            { "data": "userEmailId", "title": "User Email", "width": "70px" },
            { "data": "password", "title": "Password", "width": "70px" },
            { "data": "confirmpassword", "title": "Confirm Password", "width": "70px" },
            { "data": "role", "title": "Role", "width": "70px" },
            {
                "data": "id",
                "width": "20%",
                "title": "Action",
                "render": function (data) {
                    return `
        <div class="row" style="width: 170px; margin: 0; padding: 0;">
            <div class="col-6" style="padding: 0;">
                <button type="button" class="dt-btn-approve btn btn-primary btn-sm btn-block" style="width: 80px;" onclick="EditEmployee(${data})">Edit</button>
            </div>
            <div class="col-6" style="padding: 0;">
                <button type="button" class="dt-btn-reject btn btn-danger btn-sm btn-block" style="width: 80px;" onclick="CancelEmployee(${data})">Delete</button>
            </div>
        </div>`;
                }
            }
        ]
    });

    $('#tblemployee_wrapper').find(".row:first").prop("style");
    $('#tblemployee_wrapper').find("select[name='tblemployee_length']").prop("style", "margin-top:5%; margin-left:10px; width:30% !important;");
}

function fnAddNewMaster() {
    $('#divEmployeeIndex').hide();
    $('#divAddEmployee').show();
    $('#Name').val("");
    $('#UserEmailId').val("");
    $('#Password').val("");
    $('#ConfirmPassword').val("");
    $('#Role').val("");
}

function ViewEmployeedata(Data) {
    $('#divEmployeeIndex').hide();
    $('#divAddEmployee').show();
    $.ajax({
        type: "GET",
        url: "/Admin/GetEmployeeDetailById",
        data: { Id: Data },
        success: function (data) {
            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
            } else {
                if (data.employee) {
                    $('#Id').val(data.employee.Id);
                    $('#Name').val(data.employee.Name);
                    $('#UserEmailId').val(data.employee.UserEmailId);
                    $('#Password').val(data.employee.Password);
                    $('#Confirmpassword').val(data.employee.Confirmpassword);
                    $('#Role').val(data.employee.Role);
                } else {
                    console.error("Employee data is missing in the response.");
                    $('._CustomMessage').text("Employee data is missing.");
                    $('#errorPopup').modal('show');
                }
            }
        },
        error: function () {
            console.error("An error occurred while updating details.");
            $('._CustomMessage').text("An error occurred while updating details.");
            $('#errorPopup').modal('show');
        }
    });
}

function fnAddNewEmployee() {
    $('#divAddEmployee').show();
    $('#divEmployeeIndex').hide();
    $('#Name').val("");
    $('#UserEmailId').val("");
    $('#Password').val("");
    $('#Confirmpassword').val("");
}

function AddUpdateEmployee() {
    var employee = {
        Name: $("#Name").val(),
        UserEmailId: $("#UserEmailId").val(),
        Password: $("#Password").val(),
        Confirmpassword: $("#Confirmpassword").val(),


    };
    var name = $("#Name").val();
    var userEmailId = $("#UserEmailId").val();
    var password = $("#Password").val();
    var confirmPassword = $("#Confirmpassword").val();

    // Check if any field is empty
    if (!name || !userEmailId || !password || !confirmPassword) {
        alert("All fields are required. Please fill in all the fields.");
        return; // Stop execution if validation fails
    }

    // Check if Password and Confirm Password match
    if (password !== confirmPassword) {
        alert("Passwords do not match.");
        return; // Stop execution if passwords do not match
    }

    $.ajax({
        type: "POST",
        url: '/Admin/AddUpdateEmployee',
        data: JSON.stringify(employee), 
        contentType: "application/json",
        success: function (data) {
            if (data.IsSuccess) {
                LoadEmployeedetails();
                $('#divAddEmployee').show();
                $('#divEmployeeIndex').hide();
                
            } else {
            //    alert(data.Message);
            }
        },
        error: function (errormessage) {
            console.error("An error occurred while saving employee details.");
        }
    });
}



function EditEmployee(id) {
    console.log("EditEmployee called with ID:", id);
    $.ajax({
        type: "GET",
        url: "/Admin/GetEmployeeDetailById",
        data: { Id: id },
        success: function (response) {
            console.log(response);
            if (response && response.isSuccess && response.employee) {
                $('#Id').val(response.employee.id); 
                $('#Name').val(response.employee.name);
                $('#UserEmailId').val(response.employee.userEmailId);
                $('#Password').val(response.employee.password);
                $('#Confirmpassword').val(response.employee.confirmpassword);
                $('#Role').val(response.employee.role);
                $('#divAddEmployee').data('Id', id);
                $('#divAddEmployee').show();
                $('#divEmployeeIndex').hide();

            } else {
                console.error("Employee data is missing or there was an error in the response.");
                $('._CustomMessage').text(response.message || "Error retrieving employee data.");
                $('#errorPopup').modal('show');
            }
        },
        error: function () {
            console.error("An error occurred while fetching employee details.");
            $('._CustomMessage').text("Error retrieving employee details.");
            $('#errorPopup').modal('show');
        }
    });
}

$('#btnEditEmployee').on('click', function () {
    var updatedEmployeeData = {
        id: $('#divAddEmployee').data('Id'),
        name: $('#Name').val(),
        userEmailId: $('#UserEmailId').val(),
        password: $('#Password').val(),
        confirmpassword: $('#Confirmpassword').val(),
        role: $('#Role').val()
    };

    $.ajax({
        type: "POST",
        url: "/Admin/AddUpdateEmployee",
        data: JSON.stringify(updatedEmployeeData),
        contentType: "application/json",
        success: function (response) {
            if (response.isSuccess) {
                LoadEmployeedetails();
                $('#divAddEmployee').hide();
                $('#divEmployeeIndex').show();
            } else {
                $('._CustomMessage').text(response.message);
                $('#successPopup').modal('show');
            }
        },
        error: function () {
            console.error("An error occurred while updating employee.");
        }
    });
});

function CancelEmployee(employeeId) {
    if (confirm("Are you sure you want to cancel this Employee?")) {
        $.ajax({
            type: "POST",
            url: "/Admin/CancelEmployee", 
            data: { id: employeeId },
            success: function (response) {
                if (response.isSuccess) {
                    LoadEmployeedetails();  // Reload the leave data
                    $('#divAddEmployee').hide();  // Show success message
                    $('#divEmployeeIndex').show();  // Show success message
                } else {
                    $('._CustomMessage').text(response.message);
                    $('#successPopup').modal('show');
                }
            },
            error: function () {
                console.error("An error occurred while canceling leave.");
            }
        });
    }
}



