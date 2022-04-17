(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();

$(document).ready(function () {
    let email = $("#emailOtp").val();
    
    $("#sendOtp").click(function () {
        console.log(email);
        $.ajax({
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            url: `https://localhost:44325/api/accounts/forgotpassword/${email}`,
            type: "PUT",
            contentType: "application/json",
            dataType: "json"
        }).done((result) => {
            Swal.fire(
                'Success!',
                'OTP code has send to your email!',
                'success'
            ).then((result) => {
                if (result.isConfirmed) {
                    location.href = "/ChangePassword";
                }
            })
        })
    });
});