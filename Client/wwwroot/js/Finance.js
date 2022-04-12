// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//list data employee overtime
$(document).ready(function () {
    let table = $("#tblOvertime").DataTable({
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
            {
                data: null,
                render: function (data, type, row) {
                    return `<button class="btn btn-info" data-toggle="modal" data-target="#modalDetail" onclick="GetEmployee('${data.nik}')"><i class="fa-solid fa-info"></i></button>`;
                }
            }
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

    table.buttons().container().appendTo("#tblOvertime .col-md-6:eq(0)");
});