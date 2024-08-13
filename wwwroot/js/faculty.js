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
                    location.reload();
                    toastr.success("Faculty Successfully deleted");
                }
            })
        }
    });
}