


    $('.deleteblog').click(function (event) {


        event.preventDefault();
            $('#deletemodal').attr('href', '');
            var href = this.getAttribute('href');
            $('#deletemodal').attr('href', href);
            console.log(this.getAttribute('href'));


        });

