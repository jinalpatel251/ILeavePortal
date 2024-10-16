document.addEventListener('DOMContentLoaded', function (event) {
    LoadLeavedetails();
});

function LoadLeavedetails() {
    $.ajax({
        type: "POST",
        url: "/ApplyLeave/LoadApplyLeave",
        data: {},
        success: function (Obj) {
            if (!Obj.isSuccess) {
                $('._CustomMessage').text(Obj.message);
                $('#divapplyleaveindex').show();
            } else {
                LoadLeavedata(Obj.list);
                $('#divApplyLeave').hide();
            }
        },
        error: function () {
            console.error("An error occurred while loading leave details.");
        }
    });
}

function LoadLeavedata(List) {
    List.sort(function (a, b) {
        return b.id - a.id;
    });

    $('#tblapplyleave').DataTable({
        "pageLength": 10,
        "processing": true,
        "destroy": true,
        "aaData": List,
        "order": [[0, "DESC"]],
        "columns": [
            { "data": "id", "title": "Id", "width": "70px" },
            { "data": "leaveType", "title": "LeaveType", "width": "70px" },
            { "data": "startDate", "title": "StartDate", "width": "70px" },
            { "data": "endDate", "title": "EndDate", "width": "70px" },
            { "data": "reason", "title": "Reason", "width": "70px" },
            {
                "data": "status",
                "title": "Status",
                "width": "70px",
                "render": function (data, type, row) {
                    if (data === "Approved") {
                        return `<span style="background-color: green; color: white; padding: 5px 10px; border-radius: 4px;">Approved</span>`;
                    }
                    if (data === "Rejected") {
                        return `<span class="btn btn-danger" style="color: white; padding: 5px 10px; border-radius: 4px;">Rejected</span>`;
                    }
                    if (data === "Pending") {
                        return `<span style="background-color: yellow; color: black; padding: 5px 10px; border-radius: 4px;">Pending</span>`;
                    }
                    if (data === "Cancelled") {
                        return `<span style="background-color: gray; color: white; padding: 5px 10px; border-radius: 4px;">Cancelled</span>`;
                    }
                    return `<span>${data}</span>`; 
                }
            },

            {
                "data": "id",
                "width": "10%",
                "title": "Action",
                "class": "text-center",
                "render": function (data, type, row) {
                    if (row.status === "Approved") {
                        return `<span style="background-color: green; color: white; padding: 5px 10px; border-radius: 4px;">Approved</span>`;
                    }
                    if (row.status === "Rejected") {
                        return `<span class="btn btn-danger "  style="color: white;">Rejected</span>`;
                    }
                    return `
            <div class="row" style="width:170px">
                <div class="col-xl-5">
                    <button type="button" class="dt-btn-approve btn btn-primary btn-block" onclick="EditLeave('${data}')">Edit</button>
                </div>
                <div class="col-xl-5 d-flex justify-content-center" style="width:auto">
                    <button type="button" class="dt-btn-reject btn btn-danger btn-block" onclick="CancelLeave('${data}')">Delete</button>
                </div>
            </div>`;
                }
            }
        ]
    });

    $('#tblapplyleave_wrapper').find(".row:first").prop("style");
    $('#tblapplyleave_wrapper').find("select[name='tblapplyleave_length']").prop("style", "margin-top:5%; width:35% !important;");
}

function fnAddNewMaster() {

    $('#divapplyleaveindex').hide();
    $('#divApplyLeave').show();
    $('#LeaveType').val("");
    $('#StartDate').val("");
    $('#EndDate').val("");
    $('#Reason').val("");
    $('#Status').val("");
}


function AddUpdateLeave() {
    var applyLeave = {
        LeaveType: $("#LeaveType").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        Reason: $("#Reason").val(),
    };

    $.ajax({
        type: "POST",
        url: '/ApplyLeave/AddUpdateLeave',
        data: JSON.stringify(applyLeave), // Convert employee object to JSON
        contentType: "application/json", // Set content type to JSON
        success: function (data) {
            $('#divApplyLeave').hide();
            $('#divapplyleaveindex').show();
            if (data.IsSuccess) {
                $('#divApplyLeave').hide();
                $('#divapplyleaveindex').show();

                // Handle success
                //alert(data.Message);
            } else {
                //    alert(data.Message);
            }
        },
        error: function (errormessage) {
            console.error("An error occurred while saving employee details.");
        }
    });
}

function EditLeave(id) {
    console.log("EditEmployee called with ID:", id);
    $.ajax({
        type: "GET",
        url: "/ApplyLeave/GetLeaveDetailById",
        data: { Id: id },
        success: function (response) {
            console.log(response);
            if (response && response.isSuccess && response.applyLeave) {
                $('#Id').val(response.applyLeave.id); // Ensure IDs match
                $('#LeaveType').val(response.applyLeave.leaveType);
                $('#StartDate').val(response.applyLeave.startDate);
                $('#EndDate').val(response.applyLeave.endDate);
                $('#Reason').val(response.applyLeave.reason);
               
                $('#divApplyLeave').data('Id', id);
                $('#divApplyLeave').show();
                $('#divapplyleaveindex').hide();

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

$('#btnEditLeave').on('click', function () {
    var updatedLeaveData = {
        id: $('#divApplyLeave').data('Id'),
        leaveType: $('#LeaveType').val(),
        startDate: $('#StartDate').val(),
        endDate: $('#EndDate').val(),
        reason: $('#Reason').val(),
       
    };

    $.ajax({
        type: "POST",
        url: "/ApplyLeave/AddUpdateLeave",
        data: JSON.stringify(updatedLeaveData),
        contentType: "application/json",
        success: function (response) {
            if (response.isSuccess) {
                LoadLeavedetails();
                $('#divApplyLeave').hide();
                $('#divapplyleaveindex').show();
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


function CancelLeave(leaveId) {
    if (confirm("Are you sure you want to cancel this leave?")) {
        $.ajax({
            type: "POST",
            url: "/ApplyLeave/CancelLeave",  // Update with your backend endpoint
            data: { id: leaveId },
            success: function (response) {
                if (response.isSuccess) {
                    LoadLeavedetails();  // Reload the leave data
                    $('#successPopup').modal('show');  // Show success message
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