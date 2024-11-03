$(document).ready(function() {
    var slider = $("#slider")[0]; 

    var startMin = $("#slider").data("min") || 0; // Giá trị mặc định là 0 nếu không có data-min
    var startMax = $("#slider").data("max") || 20; // Giá trị mặc định là 20 nếu không có data-max

    noUiSlider.create(slider, {
        start: [startMin, startMax], // Thiết lập giá trị start từ data-attributes
        connect: true,
        range: {
            min: 0,
            max: 20,
        },
        step: 0.1,
        format: {
            to: function(value) {
                return value;
            },
            from: function(value) {
                return value;
            }
        },
    });

    slider.noUiSlider.on("update", function(values) {
        $("#minPriceValue").text('Từ ' + values[0]);
        $("#maxPriceValue").text(values[1] + ' triệu' + (values[1] >= 20 ? '+' : ''));
        $("#minPrice").val(values[0] * 1_000_000);
        $("#maxPrice").val(values[1] >= 20 ? '' : values[1] * 1_000_000);
    });
});
