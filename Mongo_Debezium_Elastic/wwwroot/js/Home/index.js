$("#users").select2({
    placeholder: "type to find user",
    theme: "bootstrap4",
    minimumResultsForSearch: 5,
    allowClear: true,
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