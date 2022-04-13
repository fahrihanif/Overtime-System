$(document).ready(function () {
    let table = $("#tblOvertime").DataTable({
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
                    } else if (data.status === "Approval Manager") {
                        return `<span class="badge badge-pill badge-info">${data.status}</span>`;
                    } else {
                        return `<span class="badge badge-pill badge-warning">${data.status}</span>`;
                    }
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button class="btn btn-info" data-toggle="modal" data-target="#modalDetail" onclick="GetDetailOvertimes('${data.nik}','${data.submit}','${data.status}')"><i class="fa-solid fa-info"></i></button>`;
                }
            }
        ]
    });
});

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

        $(document).on('click', '.approval', function (event) {
            let info = this.id;

            let ovr = new Object();
            ovr.id = result.list[0].overtimeId;
            ovr.submitDate = result.submitDate;
            ovr.paid = result.paid;
            ovr.type = result.type

            if (info === "accepted") {
                ovr.status = 2;
            } else {
                ovr.status = 3;
            }

            Approval(ovr);
        });
    }).fail((error) => {
        console.log(error)
    })
}

function Approval(ovr) {
    $.ajax({
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        type: "PUT",
        url: "https://localhost:44325/api/overtimes/",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(ovr)
    }).done((result) => {
        Swal.fire({
            title: 'Updated!',
            icon: 'success',
            showConfirmButton: false,
            timer: 1500
        })
        $('#tblOvertime').DataTable().ajax.reload();
        $('#modalDetail').modal('hide');
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: `Something went wrong! ${error.responseJSON.message}`,
        })
        $('#tblOvertime').DataTable().ajax.reload();
    })
}