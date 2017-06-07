var service = angular.module('testService', []);

service.factory('testservice', function ($resource) {
    return $resource('/api/WaiterAPI/:action/:id/:typeid/:tableid/', { id: '@id' }, {
        GetOrder: {
            method: 'GET', isArray: true, params: { action: 'GetOrder' }
        },
        DetailOrder: {
            method: 'GET', params: { action: 'DetailOrder' }
        },
        ServedOrder: {
            method: 'POST', params: { action: 'ServedOrder' }
        },
        CancelOrder: {
            method: 'POST', params: { action: 'CancelOrder' }
        },
        GetTable: {
            method: 'POST', isArray: true, params: { action: 'Table' }
        },
        PayOrder: {
            method: 'POST', params: { action: 'PayOrder' }
        },
        GetMenu: {
            method: 'GET', params: { action: 'GetMenu' }
        },
        NewOrder: {
            method: 'POST', params: { action: 'CreateOrder' }
        },
        AddOrder: {
            method: 'POST', params: { action: 'AddOrder' }
        },
        EditOrder: {
            method: 'POST', params: { action: 'EditOrder' }
        },
        EditAllOrder: {
            method: 'POST', params: { action: 'EditAllOrder' }
        },
        InfoRestaurant: {
            method: 'GET', params: { action: 'InfoRestaurant' }
        }
    });
});

service.factory('kitchenservice', function ($resource) {
    return $resource('/api/KitchenAPI/:action/:id', { id: '@id' }, {
        GetAllOrderItem: {
            method: 'GET', isArray: true, params: { action: 'GetAllOrderItem' }
        },
        CancelOrderItem: {
            method: 'POST', params: { action: 'CancelOrderItem' }
        },
        CookOrderItem: {
            method: 'POST', params: { action: 'CookOrderItem' }
        },
        FinishOrderItem: {
            method: 'POST', params: { action: 'FinishOrderItem' }
        },
        GetAllOrderItemCateByOrder: {
            method: 'GET', isArray: true, params: { action: 'GetAllOrderItemCateByOrder' }
        },
        GetAllOrder: {
            method: 'GET', isArray: true, params: { action: 'GetAllOrder' }
        },
        GetOrderItemByOrderID: {
            method: 'GET', isArray: true, params: { action: 'GetOrderItemByOrderID' }
        },
        GetAllOrderItemPrint: {
            method: 'GET', params: { action: 'GetAllOrderItemPrint' }
        },
        GetOrderItemPrint: {
            method: 'GET', params: { action: 'GetOrderItemPrint' }
        },
        FinishAllOrderItem: {
            method: 'POST', params: {action: 'FinishAllOrderItem'}
        },
        CookAllOrderItem: {
            method: 'POST', params: {action : 'CookAllOrderItem'}
        }
    });
});



