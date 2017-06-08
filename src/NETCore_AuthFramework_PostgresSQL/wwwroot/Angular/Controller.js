var controller = angular.module('testController', []);
controller.controller('testcontroller', function ($scope, testservice, kitchenservice, $timeout) {
    var testService = new testservice();
    var kitchenService = new kitchenservice();

    $scope.grandTotal = 0;

    $scope.detailorder = {};
    $scope.dataTable = [];
    $scope.order = testservice.GetOrder();
    $scope.Name = '';
    $scope.typeID = 0;
    $scope.tableID = 0;
    $scope.orderID = 0;
    $scope.isAddOrder = false;
    $scope.isEditMode = false;
    $scope.isEditAllMode = false;

    console.log($scope.order);  

    $scope.reset = function () {
        $scope.Name = '';
        $scope.typeID = 0;
        $scope.tableID = 0;
        $scope.orderID = 0;
        $scope.isAddOrder = false;
        $scope.isEditMode = false;
        $scope.isEditAllMode = false;
        $scope.detailorder = {};
    }

    $scope.addOrder = function (id) {
        console.log(id);
        $scope.orderID = id;
        $scope.isAddOrder = true;
        //$scope.selectedOrder = angular.copy($scope.detailorder);
        $scope.GetMenu($scope.detailorder.TableID, $scope.detailorder.TableName, $scope.detailorder.TypeID);
    };

    $scope.DetailOrder = function (id) {
        console.log(id);
        $scope.isEditMode = false;
        $scope.isEditAllMode = false;
        $scope.pay = false;
        $scope.Name = '';
        testService.$DetailOrder({ id: id }, function (data) {
            $scope.test = false;
            $scope.detailorder = data;
            $scope.Name = data.Name;

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
        $scope.isEditMode = true;
        //console.log($scope.test);
    };

    $scope.editall = function () {
        $scope.test = true;
        $scope.isEditAllMode = true;
        console.log('trigger');
        console.log($scope.test);
    };

    $scope.EditQtyPlus = function (index) {

        $scope.detailorder.OrderItem[index].Qty++;

    };

    $scope.EditQtyMinus = function (index) {
        if ($scope.detailorder.OrderItem[index].Qty > 1) {
            $scope.detailorder.OrderItem[index].Qty--;
        }
    };

    $scope.save = function () {

        $scope.new = {
            "OrderID": $scope.detailorder.OrderID,
            "OrderItem": $scope.detailorder.OrderItem
        };
        //api post disini
        //console.log($scope.detailorder);
        testservice.EditOrder($scope.new, function (data) {
            $scope.new = {};
            $scope.isEditMode = false;
            $scope.isEditAllMode = false;
            $scope.order = testservice.GetOrder();
            $scope.detailorder = null;
        });
        //console.log($scope.test);
    };

    $scope.saveall = function () {

        $scope.new = {
            "OrderID": $scope.detailorder.OrderID,
            "OrderItem": $scope.detailorder.OrderItem
        };
        //api post disini
        //console.log($scope.detailorder);
        testservice.EditAllOrder($scope.new, function (data) {
            $scope.new = {};
            $scope.isEditMode = false;
            $scope.isEditAllMode = false;
            $scope.order = testservice.GetOrder();
            $scope.detailorder = null;
        });
        //console.log($scope.test);
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

    $scope.print = function (div) {
        testservice.InfoRestaurant(function (data) {
            $scope.info = data;
            var printContents = document.getElementById(div).innerHTML;

            var popupWin = window.open("", "");
            
            popupWin.document.write('<html><head><title>Restaurant</title>'
                + '<link href="/lib/bootstrap/dist/css/bootstrap.css" rel="stylesheet" />'
                + '</head><center>'
                + '<div class="row"><h4>' + $scope.info.HeaderLine1 + '</h4></div>'
                + '<div class="row"><h4>' + $scope.info.HeaderLine2 + '</h4></div>'
                + '<div class="row"><h4>' + $scope.info.HeaderLine3 + '</h4></div>'
                + '</center>' + printContents + '<center>'
                + '<div class="row"><h4>' + $scope.info.FooterLine1 + '</h4></div>'
                + '<div class="row"><h4>' + $scope.info.FooterLine2 + '</h4></div>'
                + '<div class="row"><h4>' + $scope.info.FooterLine3 + '</h4></div>'
                + '</body></html>');
             
            popupWin.document.close();
            popupWin.print();
            popupWin.close();
        })
       
    };

    $scope.GoPay = function (id, uang, total) {

        console.log('DetailOrder');
        console.log(id + "," + uang + "," + total);
        if (uang >= total) {
            console.log("uang cukup");
            testservice.PayOrder({ id: id }, function (data) {
                if (data.Status === true) {
                    console.log("Success");
                    $scope.print('DetailOrder');
                    $timeout(function () {
                        $scope.order = testservice.GetOrder();
                        $scope.detailorder = null;
                    },500)
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
        if ($scope.detailorder.OrderItem.length === 1) {
            alert('Order Tidak Bisa Di cancel');
            $scope.DetailOrder(orderId);
            $scope.order = testservice.GetOrder();
        } else {
            testservice.CancelOrder({ id: orderItemId }, function (data) {
                if (data.Status === true) {
                    $scope.detailorder = {};
                    $scope.DetailOrder(orderId);
                    $scope.order = testservice.GetOrder();
                }
                //$scope.status = data;
                console.log(data);
            });
        }
       

    };

    $scope.GetTable = function (typeid) {
        $scope.typeID = typeid;
        console.log($scope.typeID);
        testservice.GetTable({}, function (data) {
            $scope.dataTable = data;
            console.log(data);
        });
    };

    $scope.baru = {};
    $scope.GetMenu = function (id, tablename, typeid) {

        $scope.orderedItems = [];
        //$scope.Name = '';
        //
        $scope.tableID = id;
        $scope.typeID = typeid;

        if (!$scope.isAddOrder) {
            $scope.Name = '';
            //$scope.orderedItems = $scope.selectedOrder.OrderItem;
        }

        $scope.baru = { "TableID": id, "TableName": tablename };
        //
        console.log("tes");
        testservice.GetMenu({}, function (data) {
            $scope.menu = data;

            console.log('menu', data);
        });
    };

    $scope.orderedItems = [];
    

    $scope.addqty = function (item) {

        if (item.Notes === null)
            item.Notes = '';

        $scope.cek = false;
        angular.forEach($scope.orderedItems, function (obj) {
            if (item.MenuID === obj.MenuID) {
                $scope.cek = true;
                obj.Qty = obj.Qty + 1;
            }
        });
        if ($scope.cek === false) {
            $scope.orderedItems.push(item);
            item.Qty = item.Qty + 1;
        }
    };

    $scope.delqty = function (MenuID, index) {
        console.log(MenuID);
        angular.forEach($scope.orderedItems, function (obj) {
            if (MenuID === obj.MenuID) {
                $scope.cek = true;
                if (obj.Qty === 1) {
                    $scope.orderedItems.splice(index, 1);
                } else {
                    obj.Qty = obj.Qty - 1;
                }
            }
        });
    };

    $scope.new = {};
    $scope.CreateOrder = function () {
        console.log('test');
        //console.log($s, tableid)
        
            if ($scope.isAddOrder) {
               
                    $scope.new = {
                        "OrderID": $scope.orderID,
                        "OrderItem": $scope.orderedItems
                    }
                    //api post disini

                    testservice.AddOrder($scope.new, function (data) {
                        console.log($scope.new);
                        console.log(data);
                        $scope.order = testservice.GetOrder();
                        $scope.detailorder = null;
                        $scope.isAddOrder = false;
                        $scope.selectedOrder = {};
                        $scope.new = {};
                        $('#myModal2').modal('hide');
                    })                
            } else {

                    $scope.new = {
                        "Name": $scope.Name,
                        "TypeID": $scope.typeID,
                        "TableID": $scope.tableID,
                        "OrderItem": $scope.orderedItems
                    };
                    console.log($scope.new);

                    console.log('trigger+', $scope.Name);
                    testService = new testservice();

                    testService.Name = $scope.Name;
                    testService.TypeID = $scope.typeID;
                    testService.TableID = $scope.tableID;
                    testService.OrderItem = $scope.orderedItems;
                    
                    
                //testservice.data = $scope.new;
                    if (testService.TypeID === 2) {
                        if ($scope.Name == '') {
                            alert('Silahkan isi Nama');
                        } else {
                            testService.$NewOrder().then(function (data) {
                                console.log(data);
                                $scope.order = testservice.GetOrder();
                                $scope.detailorder = null;
                                $('#myModal2').modal('hide');
                            });
                        }
                    } else {
                        testService.$NewOrder().then(function (data) {
                            console.log(data);
                            $scope.order = testservice.GetOrder();
                            $scope.detailorder = null;
                            $('#myModal2').modal('hide');
                        });
                    }
                                
        }
    };

    // create function order
    // API post model order

    // create function

    //$scope.newOrder = {
    //    TableID : tableid,
    //    TypeID : typeid,
    //    OrderItem : $scope.orderedItems
    //};

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
        kitchenservice.GetOrderItemPrint({ id: id }, function (obj) {
            $scope.kitchenprint = obj;
            console.log($scope.kitchenprint);
            $timeout(function () {
                $scope.print('printkitchen');
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
            }, 500);
            

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
        kitchenservice.GetAllOrderItemPrint({ id: id }, function (obj) {
            $scope.kitchenprint = obj;
            console.log($scope.kitchenprint);
            $timeout(function () {
                $scope.print('printkitchen');
            }, 500);
            kitchenservice.FinishAllOrderItem({ id: id }, function (response) {
                console.log(response);
                $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
            });
        });
       

    };

    $scope.cookall = function (id) {
        console.log(id);
        kitchenservice.CookAllOrderItem({ id: id }, function (data) {
            console.log(data);
            $scope.kitchenorderitemcatebyorder = kitchenservice.GetAllOrderItemCateByOrder();
        });
    };
});
