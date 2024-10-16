//document.addEventListener('DOMContentLoaded', function () {
//    //    loadData();
//});

//// Load Data function
////function loadData() {

////    $.ajax({
////        url: "/AddEmployee/List",
////        type: "GET",
////        contentType: "application/json;charset=utf-8",
////        dataType: "json",
////        success: function (result) {
////            var html = '';
////            $('#divEmployee').modal('hide');
////            $.each(result, function (key, item) {
////                html += '<tr>';
////                html += '<td>' + item.id + '</td>';
////                html += '<td>' + item.name + '</td>';
////                html += '<td>' + item.userEmailId + '</td>';
////                html += '<td>' + item.password + '</td>';
////                html += '<td>' + item.confirmpassword + '</td>';
////                html += '<td>' + item.role + '</td>';
////                html += '<td><a href="#" onclick="return GetbyId(' + item.id + ')">Edit</a> | <a href="#" onclick="Delete(' + item.id + ')">Delete</a></td>';
////                html += '</tr>';
////            });
////            $('.tbody').html(html);
////        },
////        error: function (errormessage) {
////            //alert(errormessage.responseText);
////        }
////    });
////}

//// Add Data Function


//// Function for getting the Data Based upon Employee ID
////function GetbyId(Id) {
////    $('#Id').css('border-color', 'lightgrey');
////    //$('#Name').css('border-color', 'lightgrey');
////    $('#UserEmailId').css('border-color', 'lightgrey');
////    $('#Password').css('border-color', 'lightgrey');
////    //$('#Confirmpassword').css('border-color', 'lightgrey');
////    //$('#Role').css('border-color', 'lightgrey');
////    $.ajax({
////        url: "/Login/GetbyId/" + Id,
////        type: "GET",
////        contentType: "application/json;charset=UTF-8",
////        dataType: "json",
////        success: function (result) {
////            $('#Id').val(result.id);
////            $('#Name').val(result.name);
////            $('#UserEmailId').val(result.userEmailId);
////            $('#Password').val(result.password);
////            $('#ConfirmPassword').val(result.Confirmpassword);
////            $('#Role').val(result.Role);


////            $('#btnUpdate').show();
////            $('#btnAdd').hide();
////        },
////        error: function (errormessage) {
////            alert(errormessage.responseText);
////        }
////    });
////    return false;
////}

//// Function for updating employee's record
////function Update() {
////    //var res = validate();
////    //if (res == false) {
////    //    return false;
////    //}
////    var loginObj = {
////        Id: $('#id').val(),
////        Name: $('#Name').val(),
////        userEmailId: $('#UserEmailId').val(),
////        Confirmpassword: $('#Confirmpassword').val(),
////        Role: $('#Role').val(),
////    };
////    //alert(empObj);
////    //console.log(empObj);
////    $.ajax({
////        url: "/Login/Update",
////        data: { login: loginObj },
////        type: "POST",
////        //contentType: "application/json;charset=utf-8",
////        dataType: "json",
////        success: function (result) {
////            loadData();
////            $('#employeeForm').modal('hide');
////            $('#Id').val("");
////            $('#Name').val("");
////            $('#UserEmailId').val("");
////            $('#Password').val("");
////            $('#Confirmpassword').val("");
////            $('#Role').val("");
////        },
////        error: function (errormessage) {
////            //alert(errormessage.responseText);
////        }
////    });
////}

//// Function for deleting employee's record
////function Delete(Id) {
////    var ans = confirm("Are you sure you want to delete thi Record?");
////    if (ans) {
////        $.ajax({
////            url: "/Login/Delete/" + Id,
////            type: "POST",
////            contentType: "application/json;charset=UTF-8",
////            dataType: "json",
////            success: function (result) {
////                //console.log(result);
////                $('#myModal').modal('hide');
////                loadData();
////            },
////            error: function (errormessage) {
////                //alert(errormessage.responseText);
////            }
////        });
////    }
////}                            

//// Function for clearing the textboxes
////function clearTextBox() {
////    $('#Id').val("");
////    $('#Name').val("");
////    $('#UserEmailId').val("");
////    $('#Password').val("");
////    $('Confirmpassword').val("");
////    $('#Role').val("");
////    $('#btnUpdate').hide();
////    $('#btnAdd').show();
////    $('#Name').css('border-color', 'lightgrey');
////    $('#UserEmailId').css('border-color', 'lightgrey');
////    $('#Password').css('border-color', 'lightgrey');
////    $('#Confirmpassword').css('border-color', 'lightgrey');
////    $('#Role').css('border-color', 'lightgrey');
////}

//function LoadEmployeedetails() {
//    $.ajax({
//        type: "POST",
//        url: "/Employee/LoadEmployee",
//        data: {},
//        success: function (Obj) {
//            if (!Obj.isSuccess) {
//                $('._CustomMessage').text(Obj.message);
//                $('#successPopup').modal('show');
//            } else {
//                LoadEmployeedata(Obj.list);
//                $('#divEmployee').hide();
//            }
//        },
//        error: function () {
//            console.error("An error occurred while loading leave details.");
//        }
//    });
//}
//function LoadEmployeedata(List) {
//    List.sort(function (a, b) {
//        return b.id - a.id;
//    });

//    // Initialize DataTable on the correct ID
//    $('#tblemployee').DataTable({
//        "pageLength": 10,
//        "processing": true,
//        "destroy": true,
//        "aaData": List,
//        "order": [[0, "DESC"]],
//        "columns": [
//            { "data": "id", "title": "Id", "width": "70px" },
//            { "data": "name", "title": "Name", "width": "70px" },
//            { "data": "useremailid", "title": "UserEmailId", "width": "70px" },
//            { "data": "password", "title": "Password", "width": "70px" },
//            { "data": "confirmpassword", "title": "ConfirmPassword", "width": "70px" },
//            { "data": "role", "title": "Role", "width": "70px" },


//        ]
//    });

//    $('#tblemployee_wrapper').find(".row:first").prop("style", "margin-bottom:2%");
//    $('#tblemployee_wrapper').find("select[name='tblapplyleave_length']").prop("style", "margin-top:5%; width:35% !important;");
//}
//function fnAddNewEloyee() {

//    $('#divEmployeeIndex').hide();
//    $('#divAddEmployee').show();
//    $('#Name').val("");
//    $('#UserEmailId').val("");
//    $('#Password').val("");
//    $('#Confirmpassword').val("");
//    $('#Role').val("");
//}
//function AddUpdateEmployee() {
//    var employeelist = [];
//    var Id = $("#Id").attr('value');

//    if (Id == "0") {
//        employeelist.push(Id);
//    }
//    else {
//        employeelist.push($("#Id").val());
//    }
//    s
//    employeelist.push($("#Name").val());
//    employeelist.push($("#UserEmailId").val());
//    employeelist.push($("#Password").val());
//    employeelist.push($("#Confirmpassword").val());
//    //leavelist.push($("#Status").val());


//    $.ajax({
//        type: "POST",
//        url: '/Employee/AddUpdateEmployee',
//        data: employeelist,
//        contentType: false,
//        processData: false,
//        success: function (data) {
//            if (!data.isSuccess) {
//                $('._CustomMessage').text(data.message);
//                $('#errorPopup').modal('show');
//            }
//            else {
//                $('._CustomMessage').text(data.message);
//                $('#successPopup').modal('show');

//                $("#btnSaveUpdateEmployee").text("Save/Update");
//                fnLoadData();
//                fnLoadEmployeeDetails();
//                $('#divEmployeeIndex').show();

//                $("#Id").val("");
//                $("#btnSaveUpdateemployee").removeClass("disable-ele-color");

//            }
//        },
//        error: function (errormessage) {
//            // Handle error
//        }
//    });
//}
//function EditLeave(leaveId) {
//    $.ajax({
//        type: "GET",
//        url: "/Login/GetEmployeeDetails", 
//        data: { id: employeeId },
//        success: function (employeeDetails) {
//            // Populate the modal with leave details
//            $('#editEmployeeModal').find('#name').val(employeeDetails.name);
//            $('#editEmployeeModal').find('#useremailid').val(employeeDetails.userEmailId);
//            $('#editEmployeeModal').find('#password').val(employeeDetails.password);
//            $('#editEmployeeModal').find('#confirmpassword').val(employeeDetails.confirmpassword);
//            $('#editEmployeeModal').data('employeeId', employeeId);  // Store leaveId for later use
//            $('#divAddEmployee').modal('show');
//        },
//        error: function () {
//            console.error("An error occurred while fetching employee details.");
//        }
//    });
//}
//function CancelEmployee(employeeId) {
//    if (confirm("Are you sure you want to cancel this employee?")) {
//        $.ajax({
//            type: "POST",
//            url: "/Employee/CancelEmployee",  
//            data: { id: employeeId },
//            success: function (response) {
//                if (response.isSuccess) {
//                    LoadEmployeedetails();  // Reload the leave data
//                    $('#divEmployeeIndex').modal('show');  // Show success message
//                } else {
//                    $('._CustomMessage').text(response.message);
//                    $('#successPopup').modal('show');
//                }
//            },
//            error: function () {
//                console.error("An error occurred while canceling employee.");
//            }
//        });
//    }
//}

