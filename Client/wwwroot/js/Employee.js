// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//clock picker

$(document).ready(function () {
    $("input[id=startOvertimeTxt]").clockpicker({
        placement: 'bottom',
        align: 'left',
        autoclose: true,
        default: 'now',
        donetext: "Select",
        init: function () {
            console.log("colorpicker initiated");
        },
        beforeShow: function () {
            console.log("before show");
        },
        afterShow: function () {
            console.log("after show");
        },
        beforeHide: function () {
            console.log("before hide");
        },
        afterHide: function () {
            console.log("after hide");
        },
        beforeHourSelect: function () {
            console.log("before hour selected");
        },
        afterHourSelect: function () {
            console.log("after hour selected");
        },
        beforeDone: function () {
            console.log("before done");
        },
        afterDone: function () {
            console.log("after done");
        }
    });


    $("input[id=endOvertimeTxt]").clockpicker({
        placement: 'bottom',
        align: 'left',
        autoclose: true,
        default: 'now',
        donetext: "Select",
        init: function () {
            console.log("colorpicker initiated");
        },
        beforeShow: function () {
            console.log("before show");
        },
        afterShow: function () {
            console.log("after show");
        },
        beforeHide: function () {
            console.log("before hide");
        },
        afterHide: function () {
            console.log("after hide");
        },
        beforeHourSelect: function () {
            console.log("before hour selected");
        },
        afterHourSelect: function () {
            console.log("after hour selected");
        },
        beforeDone: function () {
            console.log("before done");
        },
        afterDone: function () {
            console.log("after done");
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
    let  OvertimeList = []; // list object  
    $('#tabel-overtime-temporary > tbody  > tr').each(function () { //loop in table list

        let Overtime = {}; // create new Overtime object and set its properties  
        Overtime.SubmitDate = this.cells[0].innerHTML;
        Overtime.StartOvertime = this.cells[1].innerHTML;
        Overtime.EndOvertime = this.cells[2].innerHTML;
        Overtime.Description = this.cells[3].innerHTML;

        OvertimeList.push(Overtime); // add Overtime object to list object  
    });

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


    //Validate required fields  
     // Main Error Messages Variable  

    ////validate Summary  
    //if (Overtime.Summary.trim().length == 0) {
    //    Errors += "Please provide a summary.<br>";
    //    $('#SummaryTxt').addClass("border-danger");
    //} else {
    //    $('#SummaryTxt').removeClass("border-danger");
    //}

    ////validate Year  
    //if (Overtime.Year.trim().length < 4) {
    //    Errors += "A valid Year is required.<br>";
    //    $('#YearTxt').addClass("border-danger");
    //} else {
    //    $('#YearTxt').removeClass("border-danger");
    //}

    //if (Errors.length > 0) {//if errors detected then notify user and cancel transaction  
    //    ShowMsn(Errors);
    //    return false; //exit function  
   /* }*/
    //end validation required  

    //Validate no duplicated Titles  
    //var ExistTitle = false; // < -- Main indicator  
    //$('#table-information > tbody  > tr').each(function () {
    //    var Title = $(this).find('.TitleCol').text(); // get text of current row by class selector  
    //    if (Overtime.Title.toLowerCase() == Title.toLowerCase()) { //Compare provided and existing title  
    //        ExistTitle = true;
    //        return false;
    //    }
    //});

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
    //Add Overtime if title is not duplicated otherwise show error  
   
    //    ClearMsn();
        //Create Row element with provided data  
         // Enable submit button  
    
   
}

// clear all textboxes inside form  
function ClearForm() {
    $('#form-isian input[type="text"]').val('');
}

//Msn label for notifications  
function ShowMsn(message) {
    $('#Msn').html(message);
}
//Clear text of Msn label  
function ClearMsn() {
    $('#Msn').html("");
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


//table data overtime 
$(document).ready(function () {
    let table = $("#listOvertime").DataTable({
        "ajax": {
            "url": "https://localhost:44325/api/overtimes/list/",
            "dataType": "Json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "nik" },
            { "data": "submit" },
            { "data": "total" },
            { "data": "paid" },
            { "data": "type" },
            { "data": "status" },
        ],
        dom: `<'row'<'col-md-2'l><'col-md-5'B><'col text-right'f>>
              <'row'<'col-md-12'tr>>
              <'row'<'col-md-5'i><'col-md-7'p>>`,
        buttons: [
            {
                extend: 'collection',
                text: '<i class="fa-solid fa-file-export"></i>',
                className: 'btn btn-primary',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fa-solid fa-file-excel"></i> excel',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'csv',
                        text: '<i class="fa-solid fa-file-csv"></i> csv',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'pdf',
                        text: '<i class="fa-solid fa-file-pdf"></i> pdf',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'print',
                        text: '<i class="fa-solid fa-print"></i> print',
                        exportOptions: {
                            columns: ':visible'
                        }
                    }
                ]
            },
            {
                extend: 'colvis',
                text: `<i class="fa-solid fa-table-columns"></i>`,
                columns: ':not(.noVis)',
                className: 'btn btn-primary',
            }
        ]
    });

    table.buttons().container().appendTo("#listOvertime .col-md-6:eq(0)");
});