$(document).ready(function () {
    $("input[id=startOvertimeTxt]").clockpicker({
        placement: 'bottom',
        align: 'left',
        autoclose: true,
        default: 'now',
        donetext: "Select",
        afterShow: function () {
            $(".clockpicker-minutes").find(".clockpicker-tick").filter(function (index, element) {
                return !($.inArray($(element).text(), ["0"]) != -1)
            }).remove();

            $(".clockpicker-hours").find(".clockpicker-tick").filter(function (index, element) {
                return !(parseInt($(element).html()) >= 17 && parseInt($(element).html()) <= 22);
            }).removeClass('clockpicker-tick').addClass('clockpicker-tick-disabled');
        }
    });


    $("input[id=endOvertimeTxt]").clockpicker({
        placement: 'bottom',
        align: 'left',
        autoclose: true,
        default: 'now',
        donetext: "Select",
        afterShow: function () {
            console.log("text");
            $(".clockpicker-minutes").find(".clockpicker-tick").filter(function (index, element) {
                return !($.inArray($(element).text(), ["0"]) != -1)
            }).remove();


            $(".clockpicker-hours").find(".clockpicker-tick").filter(function (index, element) {
                return !(parseInt($(element).html()) >= 17 && parseInt($(element).html()) <= 22);
            }).removeClass('clockpicker-tick').addClass('clockpicker-tick-disabled');
        }
    });
});


$(document).ready(function () {
    //set onclick events for buttons  
    $('#addOvertimeTemp').click(function () { AddOvertimeT(); });
    $('#submitOvertimeList').click(function () { PostRequest(); });
});

//Send List of Overtimes to controller  
function PostRequest() {
    //Build List object that has to be sent to controller  
    let OvertimeList = []; // list object  
    $('#tabel-overtime-temporary > tbody  > tr').each(function () { //loop in table list

        let Overtime = {}; // create new Overtime object and set its properties  
        Overtime.SubmitDate = this.cells[0].innerHTML;
        Overtime.StartOvertime = this.cells[1].innerHTML;
        Overtime.EndOvertime = this.cells[2].innerHTML;
        Overtime.Description = this.cells[3].innerHTML;

        OvertimeList.push(Overtime); // add Overtime object to list object  
    });

    console.log(OvertimeList);
    //Send list of Overtimes to controller via ajax  
    //$.ajax({
    //    url: '/home/SaveOvertimes',
    //    type: "POST",
    //    data: JSON.stringify(OvertimesList),
    //    contentType: "application/json",
    //    dataType: "json",
    //    success: function (response) {
    //        // Process response from controller  
    //        if (response === true) {
    //            ShowMsn("Overtimes have been saved successfully."); // show success notification  
    //            ClearForm(); //clear form fields  
    //            $('#table-body').empty(); // clear table items  
    //            CheckSubmitBtn(); // disable submit button  
    //        } else {
    //            ShowMsn("Ooops, an error has ocurrer while processing the transaction.");
    //        }
    //    }
    //});

}

//Add item to temp table   
function AddOvertimeT() {
    let Errors = "";
    //Create Overtime Object  
    let Overtime = {};
    Overtime.SubmitDate = $('#dateTxt').val();
    Overtime.StartOvertime = $('#startOvertimeTxt').val();
    Overtime.EndOvertime = $('#endOvertimeTxt').val();
    Overtime.Description = $('#descriptionTxt').val();

    /*validate Start & End same*/
    if (Overtime.StartOvertime == Overtime.EndOvertime) {
        Errors + "Start and End Can't Same.<br>";
        $('#startOvertimeTxt').addClass("border-danger");
        $('#endOvertimeTxt').addClass("border-danger");
        console.log(Errors);
    } else {
        let Row = $('<tr>');
        $('<td>').html(Overtime.SubmitDate).appendTo(Row);
        $('<td>').html(Overtime.StartOvertime).appendTo(Row);
        $('<td>').html(Overtime.EndOvertime).appendTo(Row);
        $('<td>').html(Overtime.Description).appendTo(Row);
        $('<td>').html("<div class='text-center'><button class='btn btn-danger btn-sm' onclick='Delete($(this))'>Remove</button></div>").appendTo(Row);

        //Append row to table's body  
        $('#tableTempList').append(Row);
        CheckSubmitBtn();
        $('#startOvertimeTxt').removeClass("border-danger");
        $('#endOvertimeTxt').removeClass("border-danger");

    }

}

// clear all textboxes inside form  
function ClearForm() {
    $('#form-isian input[type="text"]').val('');
}

//Delete selected row  
function Delete(row) { // remove row from table  
    row.closest('tr').remove();
    CheckSubmitBtn();
}

//Enable or disabled submit button  
function CheckSubmitBtn() {
    if ($('#tabel-overtime-temporary > tbody  > tr').length > 0) { // count items in table if at least 1 item is found then enable button
        $('#submitOvertimeList').removeAttr("disabled");
    } else {
        $('#submitOvertimeList').attr("disabled", "disabled");
    }
}


function GetDetailOvertimes(nik, date, status) {
    if (status === "Rejected" || status === "Approved") {
        $('.approval').hide();
    } else {
        $('.approval').show();
    }

    $.ajax({
        type: "GET",
        url: `https://localhost:44325/api/overtimes/detail/${nik}/${date}`,
    }).done((result) => {
        let text = "";
        $.each(result.list, (key, val) => {
            text += `<tr>
                        <td class="text-center">${key + 1}</td>
                        <td class="text-center">${Date.parse(val.startOvertime).toString("HH:mm")}</td>
                        <td class="text-center">${Date.parse(val.endOvertime).toString("HH:mm")}</td>
                        <td class="text-center">${val.description}</td>
                    </tr>`;
        })

        $("#nik").val(result.nik);
        $("#fullName").val(result.fullName);
        $("#submitDate").val(Date.parse(result.submitDate).toString("dd MMMM yyyy"));
        $("#salary").val(result.salary);
        $("#paid").val(result.paid);
        $("#type").val(result.type);
        $("#tblDetail").html(text);

    }).fail((error) => {
        console.log(error)
    })
}
//table data overtime 
$(document).ready(function () {
    let table = $("#listOvertime").DataTable({
        "ajax": {
            "url": "https://localhost:44325/api/overtimes/list/",
            "dataType": "Json",
            "dataSrc": ""
        },
        "columns": [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "nik" },
            {
                data: null,
                render: function (data, type, row) {
                    return Date.parse(data.submit).toString("dd MMMM yyyy");
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return data.total + " Jam";
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return "Rp. " + data.paid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    if (data.type === "Weekday") {
                        return `<span class="badge badge-pill badge-primary">${data.type}</span>`;
                    } else {
                        return `<span class="badge badge-pill badge-warning">${data.type}</span>`;
                    }
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    if (data.status === "Approved") {
                        return `<span class="badge badge-pill badge-success">${data.status}</span>`;
                    } else if (data.status === "Rejected") {
                        return `<span class="badge badge-pill badge-danger">${data.status}</span>`;
                    } else if (data.status === "ApprovalManager") {
                        return `<span class="badge badge-pill badge-info">${data.status}</span>`;
                    } else {
                        return `<span class="badge badge-pill badge-warning">${data.status}</span>`;
                    }
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button class="btn btn-info" data-toggle="modal" data-target="#modalDetailEmp" onclick="GetDetailOvertimes('${data.nik}','${data.submit}','${data.status}')"><i class="fa-solid fa-info"></i></button>`;
                }
            }
        ]
    });
});