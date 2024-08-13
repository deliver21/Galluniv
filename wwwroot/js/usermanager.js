var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/usermanager/getall' },
        "columns": [
            { data: 'name', "width": "20%" },
            { data: 'surname', "width": "20%" },
            { data: 'country.countryName', "width": "10%" },
            { data: 'speciality.specialityName', "width": "15%" },
            { data: 'faculty.facultyName', "width": "15%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                    <a href="/admin/usermanager/Upsert?id=${data}" class="btn btn-primary mx-2">
                                        <i class="bi bi-pencil-square"></i> Edit
                                    </a>
                                     <a onClick=Delete('/admin/usermanager/delete?id=${data}') class="btn btn-danger mx-2">
                                        <i class="bi bi-trash-fill"></i> Delete
                                    </a>
                    </div>`
                },
                "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    //Refresh the table after deleting an item with datatable.ajax.reload();
                    // so to access that we need to declare a variable dataTable
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}
