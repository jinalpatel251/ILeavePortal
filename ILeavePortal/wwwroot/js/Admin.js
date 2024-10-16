document.addEventListener('DOMContentLoaded', function (event) {
	Loaddetails();
});

function Loaddetails() {
	$.ajax({
		type: "POST",
		url: "/Admin/LoadLeavedetails",
		data: {},
		success: function (Obj) {
			if (!Obj.isSuccess) {
				$('._CustomMessage').text(Obj.message);
				$('#successPopup').modal('show');
			} else {
				LoadAlldata(Obj.list);
				$('#divApplyLeaveElements').hide();
			}
		},
		error: function () {
			console.error("An error occurred while loading leave details.");
		}
	});
}

function LoadAlldata(List) {
	List.sort(function (a, b) {
		return b.bookingID - a.bookingID;
	});

	// Initialize DataTable on the correct ID
	$('#tblleaveindex').DataTable({
		"pageLength": 10,
		"processing": true,
		"destroy": true,
		"aaData": List,
		"order": [[0, "DESC"]],
		"columns": [
			{ "data": "id", "title": "Id", "width": "70px" },
			{ "data": "leaveType", "title": "LeaveType", "width": "70px" },
			{ "data": "startDate", "title": "StartDate", "width": "70px" },
			{
				"data": "endDate", "title": "EndDate", "width": "70px",
				"render": function (data, type, row) {
					if (data) {
						var endDate = new Date(data);
						var day = endDate.getDate();
						var monthIndex = endDate.getMonth();
						var year = endDate.getFullYear();

						var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

						return (day < 10 ? '0' : '') + day + '-' + monthNames[monthIndex] + '-' + year;
					} else {
						return "";
					}
				}    
			},
			{ "data": "reason", "title": "Reason", "width": "70px" },
			{
				"data": "status", "title": "Status", "width": "70px",
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
				"width": "70px",
				"title": "Process",
				"class": "text-center",
				"orderable": false,
				"render": function (data, status, row) {
					if (row.status === "Pending") {
						return `<button type="button" class="dt-btn-approve" style="color: green;" onclick="Viewdata('${data.toString()}')">Process</button>`;
					} else if (row.status === "Approved") {
						return `<a style="background-color: green; color: white; padding: 5px 10px; border-radius: 4px;" type="text">Approved</a>`;
					} else if (row.status === "Cancelled") {
						return `<a style="background-color: green; color: white; padding: 5px 10px; border-radius: 4px;" type="text">Cancelled</a>`;
					} else if (row.status === "Rejected") {
						return `<a class="btn btn-danger" style=" color: white; padding: 5px 10px; border-radius: 4px;" type="text">Rejected</a>`;
					}
				}
			},
		]
	});

	$('#tbladminindex_wrapper').find(".row:first").prop("style", "margin-bottom:2%");
	$('#tbladminindex_wrapper').find("select[name='tbladminindex_length']").prop("style", "margin-top:5%; width:35% !important;");
}


function Approve() {

	var DATA = [];
	DATA.push($("#Id").val());
	DATA.push($("#LeaveType").val());
	DATA.push($("#StartDate").val());
	DATA.push($("#EndDate").val());
	DATA.push($("#Reason").val());
	DATA.push($("#Status").val());


	//DATA.push($("#BookingID").attr('value'));
	//alert($("#BookingID").attr('value'));
	$.ajax({
		type: "POST",
		url: '/Admin/LeaveApproved',
		data: { "DATA": JSON.stringify(DATA) },
		success: function (data) {
			if (!data.isSuccess) {
				$('._CustomMessage').text(data.message);
				$('#errorPopup').modal('show');
			} else {

				$('._CustomMessage').text(data.message);
				$('#successPopup').modal('show');
				window.location.replace('/Admin/Index');
			}
		},
		error: function () {
		}
	});

}

function Reject() {
	var DATA = [];
	DATA.push($("#Id").val());
	DATA.push($("#LeaveType").val());
	DATA.push($("#StartDate").val());
	DATA.push($("#EndDate").val());
	DATA.push($("#Reason").val());
	DATA.push($("#Status").val());
	$.ajax({
		type: "POST",
		url: '/Admin/LeaveRejected',
		data: { "DATA": JSON.stringify(DATA) },
		success: function (data) {
			if (!data.isSuccess) {
				$('._CustomMessage').text(data.message);
				$('#errorPopup').modal('show');
			} else {

				$('._CustomMessage').text(data.message);
				$('#successPopup').modal('show');
				window.location.replace('/Admin/Index');
			}
		},
		error: function () {
		}
	});
}

function Viewdata(Data) {
    $('#divadminindex').hide();
    $('#divApplyLeaveElements').show();
    $.ajax({
        type: "GET",
        url: "/Admin/GetLeaveDetailById",
        data: { Id: Data },
        success: function (data) {
            if (!data.isSuccess) {
                $('._CustomMessage').text(data.message);
            } else {
                $('._CustomMessage').text(data.message);
                if (data) {
                    $('#Id').val(data.id);
                    $('#LeaveType').val(data.leaveType);
                    $('#StartDate').val(data.startDate);
                    $('#EndDate').val(data.endDate);
                    $('#Reason').val(data.reason);
                    $('#Status').val(data.status);

                }
            }
        },
        error: function () {
            alert("An error occurred while updating details.");
        }
    });
}



