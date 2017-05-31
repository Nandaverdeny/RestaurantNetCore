var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice, kitchenservice, $timeout) {
    var testService = new testservice();
    var kitchenService = new kitchenservice();
    $scope.grandTotal = 0;

    $scope.detailorder = {};
    $scope.dataTable = [];
    $scope.order = testservice.GetOrder();

    $scope.DetailOrder = function (id) {
        console.log(id);
        $scope.pay = false;
        testService.$DetailOrder({ id: id }, function (data) {
            $scope.test = false;
            $scope.detailorder = data;

            $scope.calculateGrandTotal();

            console.log($scope.detailorder);
        });
    };

    $scope.calculateGrandTotal = function () {
        $scope.grandTotal = 0;
        $scope.tax = 0;
        console.log('Triggered');

        angular.forEach($scope.detailorder.OrderItem, function (item) {
            console.log('Triggered 1');
            $scope.grandTotal = $scope.grandTotal + item.Qty * item.Price;
            console.log(item);
        });
        $scope.tax = $scope.grandTotal * 0.1;
    };

    $scope.edit = function () {
        $scope.test = true;
        console.log($scope.test);
    };

    $scope.save = function () {
        $scope.test = false;
        console.log($scope.test);
    };

    $scope.serve = function (orderItemId, orderId) {
        console.log(orderItemId + "," + orderId);
        testservice.ServedOrder({ id: orderItemId }, function (data) {
            if (data.Status === true) {
                $scope.detailorder = {};
                $scope.DetailOrder(orderId);
                $scope.order = testservice.GetOrder();
            }
            //$scope.status = data;
            console.log(data);
        });

    };

    $scope.Pay = function () {
        //console.log(id + " Triggered");
        //$scope.pay = {};
        //$scope.pay = $scope.detailorder;

        //console.log($scope.pay);
        $scope.pay = true;
        angular.forEach($scope.detailorder.OrderItem, function (item) {
            if (item.Status !== 'Served') {
                console.log(item.Status);
                $scope.pay = false;
            }
        });
        if ($scope.pay === false) {
            alert('semua makanan belum dihidang');
        }

    };
    $scope.CancelPay = function () {

        $scope.pay = false;
    };

    function print(div) {
        var printContents = document.getElementById(div).innerHTML;
        var popupWin = window.open("", "");

        popupWin.document.write('<html><head><title>Restaurant</title>'
            + '<link href="/lib/bootstrap/dist/css/bootstrap.css" rel="stylesheet" />'
            + '</head><body onload="window.print()">' + printContents + '</body></html>');
        popupWin.document.close();
    }

    $scope.GoPay = function (id, uang, total) {

        console.log('DetailOrder');
        console.log(id + "," + uang + "," + total);
        if (uang >= total) {
            console.log("uang cukup");
            testservice.PayOrder({ id: id }, function (data) {
                if (data.Status === true) {
                    console.log("Success");
                    print('DetailOrder');
                    $scope.order = testservice.GetOrder();
                    $scope.detailorder = null;

                } else {
                    console.log("Failed");
                }
            });
        } else {
            console.log("uang tidak cukup");
            alert('uang tidak mencukupi');
        }
    };

    $scope.cancel = function (orderItemId, orderId) {
        console.log(orderItemId + "," + orderId);
        testservice.CancelOrder({ id: orderItemId }, function (data) {
            if (data.Status === true) {
                $scope.detailorder = {};
                $scope.DetailOrder(orderId);
                $scope.order = testservice.GetOrder();
            }
            //$scope.status = data;
            console.log(data);
        });

    };

    $scope.GetTable = function () {
        testservice.GetTable({}, function (data) {
            $scope.dataTable = data;
            console.log(data);
        });
    };

    $scope.GetMenu = function () {
        console.log("tes");
        testservice.GetMenu({}, function (data) {
            $scope.menu = data;
            console.log(data);
        });
    };

    //----------------------------------------Kitchen------------------------------------------------------------

    $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();

    $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
    $scope.kitchenorder = kitchenservice.GetAllOrder();

    $scope.CancelOrderItem = function (id) {
        console.log(id);
        kitchenservice.CancelOrderItem({ id: id }, function (data) {
            if (data.Status === true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
            }
        });

    };

    $scope.CookOrderItem = function (id) {
        console.log(id);
        kitchenservice.CookOrderItem({ id: id }, function (data) {
            if (data.Status === true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            }
        });

    };

    $scope.FinishOrderItem = function (id) {
        console.log(id);
        kitchenservice.FinishOrderItem({ id: id }, function (data) {
            if (data.Status === true) {
                console.log("Success");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            } else {
                console.log("Failed");
                $scope.kitchenorderitem = kitchenservice.GetAllOrderItem();
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            }
        });

    };

    $scope.GetOrderItemByOrderID = function (id) {
        console.log(id);
        $scope.orderitem = kitchenservice.GetOrderItemByOrderID({ id: id });
        console.log($scope.orderitem);
    };

    $scope.kitchenprint = {};
    $scope.printkitchen = function (id) {

        console.log(id);
        kitchenservice.GetOrderItemPrint({ id: id }, function (obj) {
            $scope.kitchenprint = obj;
            console.log($scope.kitchenprint);
            $timeout(function () {
                print('printkitchen');
            }, 500);


        });

    }




});