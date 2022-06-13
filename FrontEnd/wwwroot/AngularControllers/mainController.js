MyApp.controller("mainController", function ($scope, $http) {
    
    //INIT CONTROLLER
    $scope.Init = function () {
        $scope.ListaUsuarios = [];
        getUserList();
    }


    function getUserList() {
        $http.get(`${ApiServer}/Logs/GetUserList`, headersApi)
            .then(function (result) {
                $scope.ListaUsuarios = result.data;
                //console.log('lista de usuarios', $scope.ListaUsuarios);
            })
            .catch(function (result) {
                console.log("Error en consulta de lista de usuarios");
            });
    }

    function getUserInfo() {
        let id = GetUrlParam(location.href);
        $http.get(`${ApiServer}/Logs/GetUserInfo/${id}`, headersApi)
            .then(function (result) {
                $scope.MyUser = result.data;
                //console.log('lista de usuarios', $scope.ListaUsuarios);
            })
            .catch(function (result) {
                console.log("Error en consulta de informacion de usuario");
            });
    }

    $scope.InitPosts = () => {
        getUserInfo();
        let id = GetUrlParam(location.href);
        $http.get(`${ApiServer}/Logs/GetUserPosts/${id}`, headersApi)
            .then(function (result) {
                $scope.ListaPosts = result.data;
                //console.log('lista de publicaciones', $scope.ListaPosts);
            })
            .catch(function (result) {
                console.log("Error en consulta de lista de publicaciones");
            });
    }

    $scope.InitAlbums = () => {
        getUserInfo();
        let id = GetUrlParam(location.href);
        $http.get(`${ApiServer}/Logs/GetUserAlbums/${id}`, headersApi)
            .then(function (result) {
                $scope.ListaAlbums = result.data;
                //console.log('lista de Albums', $scope.ListaAlbums);
            })
            .catch(function (result) {
                console.log("Error en consulta de lista de Albums");
            });
    }

    $scope.getFotosByAlbum = (id,album) => {
        $http.get(`${ApiServer}/Logs/GetUserPhoto/${id}`, headersApi)
            .then(function (result) {
                $scope.ListaFotos = result.data;

                $("#myCarousel").carousel("pause").removeData();
                var content_indi = "";
                var content_inner = "";
                $.each($scope.ListaFotos, function (i, obj) {
                    content_indi += `<button type="button" data-bs-target="#myCarousel" data-bs-slide-to="${i}" class="" aria-label="Slide ${i + 1}"></button>`;
                    content_inner += `
                            <div class="carousel-item">
                              <img src="${obj.url}" class="d-block w-100" alt="..." style="max-height:500px !important;">
                              <div class="carousel-caption d-none d-md-block">
                                <h5>Album : ${album.title}</h5>
                                <p>${obj.title}</p>
                              </div>
                            </div>
                    `;
                });
                $('#butonsPanel').html(content_indi);
                $('#fotosContainer').html(content_inner);
                $('#butonsPanel button').first().addClass('active');
                $('#butonsPanel button').first().attr("aria-current", "true");
                
                $('#fotosContainer > .carousel-item').first().addClass('active');
                $('#myCarousel').carousel();

            })
            .catch(function (result) {
                console.log("Error en consulta de lista de Fotos");
            });
    }


    $scope.InitLogsTransactions = () => {
        $http.get(`${ApiServer}/Logs/getTransactionLogs`, headersApi)
            .then(function (result) {
                $scope.ListaTransactions = result.data;
                //console.log('lista de transactions', $scope.ListaTransactions);
            })
            .catch(function (result) {
                console.log("Error en consulta de lista de transacciones");
            });
    }

    $scope.askDeleteTransactionLogs = () => {
        if (confirm('Esta seguro que desea limpiar el Log de Transacciones?') == true) {
            clearTransactionLog();
        }
    }

    function clearTransactionLog() {
        $http.get(`${ApiServer}/Logs/ClearTransactionLogs`, headersApi)
            .then(function (result) {
                let message = result.data;

                document.getElementById('toasttittle').innerHTML = message;
                var toastLiveExample = document.getElementById('liveToast');

                var toast = new bootstrap.Toast(toastLiveExample);
                toast.show();

                setTimeout(function () {
                    location.reload();
                }, 2000);
            })
            .catch(function (result) {
                console.log("Error al limpiar el log de transacciones");
            });
    }

});