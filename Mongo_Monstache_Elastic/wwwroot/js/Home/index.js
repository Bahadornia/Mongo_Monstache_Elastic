$("#mongo-users").select2({
    placeholder: "type to find user",
    theme: "bootstrap4",
    minimumResultsForSearch: 5,
    allowClear: true,
    closeOnSelect: false,
    ajax: {
        url: "/Home/search",
        contentType: "application/json; charset=utf-8",
        delay: 500,
        data: function (params) {
            var query =
            {
                term: params.term,
            };
            return query;
        },
        processResults: function (result) {
            return {
                results: $.map(result, function (item) {
                    return {
                        id: item.id,
                        text: item.firstName + ' ' + item.lastName
                    };
                }),
            };
        }
    }

});


$("#elastic-users").select2({
    placeholder: "type to find user",
    theme: "bootstrap4",
    minimumResultsForSearch: 5,
    allowClear: true,
    closeOnSelect: false,
    ajax: {
        url: "/Home/elasticSearch",
        delay: 500,
        data: function (params) {
            var query =
            {
                term: params.term,
            };
            return query;
        },
        processResults: function (result) {
            return {
                results: $.map(result, function (item) {
                    return {
                        id: item.id,
                        text: item.firstName + ' ' + item.lastName
                    };
                }),
            };
        }
    }

});


$("#mongo-users").on('select2:closing', function (e) {
    e.preventDefault();
});
$("#elastic-users").on('select2:closing', function (e) {
    e.preventDefault();
});