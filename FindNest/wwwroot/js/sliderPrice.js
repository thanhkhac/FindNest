$(document).ready(function() {
    var slider = $("#slider")[0]; // Lấy phần tử slider bằng jQuery
    var initialLoad = 0; 

    noUiSlider.create(slider, {
        start: [0, 200],
        connect: true,
        range: {
            min: 0,
            max: 200,
        },
        step: 1,
        format: {
            to: function (value) {
                return (value * 0.1).toFixed(1);
            },
            from: function (value) {
                return value * 100000;
            }
        },
    });

    slider.noUiSlider.on("update", function (values, handle) {
        if (initialLoad === 2) {
            initialLoad ++;
            return;
        }
        console.log('Hello');
        $("#minPriceValue").text('Từ ' + values[0]);
        $("#maxPriceValue").text(values[1] + ' triệu' + (values[1] >= 20 ? '+' : ''));
        $("#minPrice").val(values[0] * 1_000_000);
        $("#maxPrice").val(values[1] >= 20 ? '' : values[1] * 1_000_000);
    });
});
