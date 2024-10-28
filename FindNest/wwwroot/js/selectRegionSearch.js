$(document).ready(function () {
    function fetchRegions(parentId, targetSelect, placeholder) {
        $.ajax({
            url: '/api/Region',
            method: 'get',
            data: {parentId: parentId},
            success: function (data) {
                targetSelect.empty();
                targetSelect.append(`<option  value=""  disabled selected>${placeholder}</option>`);
                targetSelect.append(`<option  value="" >Tất cả</option>`);
                data.forEach(function (region) {
                    targetSelect.append(`<option value ="${region.id}">${region.fullName}</option>`)
                })
                updateRegionLabel();
            },
            error: function (errorMessage) {
                console.error('Error fetching regions:', error);
            }
        })
    }

    var citySelect = $('#citySelect');
    var districtSelect = $('#districtSelect');
    var wardSelect = $('#wardSelect');
    var regionSelectLabel = $('#regionSelectLabel');

    citySelect.on('change', function () {
        wardSelect.empty();
        wardSelect.append(`<option value="" disabled selected>Chọn phường xã</option>`)
        wardSelect.append(`<option  value="" >Tất cả</option>`)

        var cityId = $(this).val();
        if (cityId) {
            fetchRegions(cityId, $('#districtSelect'), 'Chọn huyện');
        } else {
            $('#districtSelect').empty().append(`<option value="" disabled selected>Chọn quận huyện</option>`, `<option  value="" >Tất cả</option>`)
        }
        updateRegionLabel();
    })

    districtSelect.on('change', function () {
        var districtId = $(this).val();
        if (districtId) {
            fetchRegions(districtId, $('#wardSelect'), 'Chọn phường xã');
        }
        wardSelect.empty();
        wardSelect.append(`<option value="" disabled selected>Chọn phường xã</option>`)
        wardSelect.append(`<option  value="" >Tất cả</option>`)
        updateRegionLabel();
    });

    wardSelect.on('change', updateRegionLabel);

    function updateRegionLabel() {
        var selectedCity = citySelect.find(':selected');
        var selectedDistrict = districtSelect.find(':selected');
        var selectedWard = wardSelect.find(':selected');
        var hiddenRegionId = $('#regionId');
        if (selectedWard.val()) {
            regionSelectLabel.text(selectedWard.text());
            hiddenRegionId.val(selectedWard.val())
        } else if (selectedDistrict.val()) {
            regionSelectLabel.text(selectedDistrict.text());
            hiddenRegionId.val(selectedDistrict.val())
        } else {
            regionSelectLabel.text(selectedCity.text());
            hiddenRegionId.val(selectedCity.val())
        }
    }

})