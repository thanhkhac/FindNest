// Khởi tạo các thành phần DOM
var citySelect = $('#citySelect');
var districtSelect = $('#districtSelect');
var wardSelect = $('#wardSelect');
var regionSelectLabel = $('#regionSelectLabel');
var hiddenRegionId = $('#regionId');
var latitudeInput = $('#latitudeInput');
var longitudeInput = $('#longitudeInput');

$(document).ready(function () {
    function fetchRegions(parentId, targetSelect, placeholder) {
        $.ajax({
            url: '/api/Region',
            method: 'get',
            data: { parentId: parentId },
            success: function (data) {
                targetSelect.empty();
                targetSelect.append(`<option value="" disabled selected>${placeholder}</option>`);
                data.forEach(function (region) {
                    targetSelect.append(`<option value="${region.id}">${region.fullName}</option>`);
                });
                updateRegionLabel();
            },
            error: function (errorMessage) {
                console.error('Error fetching regions:', errorMessage);
            }
        });
    }

    function fetchCoordinates(query) {
        $.get(`https://nominatim.openstreetmap.org/search?format=json&q=${query}`, function (data) {
            if (data.length > 0) {
                var lat = parseFloat(data[0].lat);
                var lon = parseFloat(data[0].lon);
                updateMap(lat, lon, query);
                latitudeInput.val(lat); 
                longitudeInput.val(lon); 
            } else {
                alert('Không tìm thấy vị trí này trên bản đồ');
            }
        });
    }

    function updateRegionLabel() {
        var selectedCity = citySelect.find(':selected').text();
        var selectedDistrict = districtSelect.find(':selected').text();
        var selectedWard = wardSelect.find(':selected').text();

        if (wardSelect.val()) {
            regionSelectLabel.text(selectedWard);
            hiddenRegionId.val(wardSelect.val());
        } else if (districtSelect.val()) {
            regionSelectLabel.text(selectedDistrict);
            hiddenRegionId.val(districtSelect.val());
        } else {
            regionSelectLabel.text(selectedCity);
            hiddenRegionId.val(citySelect.val());
        }
    }

    citySelect.on('change', function () {
        wardSelect.empty().append(`<option value="" disabled selected>Chọn phường xã</option>`);
        var cityId = $(this).val();
        if (cityId) {
            fetchRegions(cityId, districtSelect, 'Chọn huyện');
            fetchCoordinates(citySelect.find(':selected').text());
        } else {
            districtSelect.empty().append(`<option value="" disabled selected>Chọn quận huyện</option>`);
        }
        updateRegionLabel();
    });

    districtSelect.on('change', function () {
        var districtId = $(this).val();
        if (districtId) {
            fetchRegions(districtId, wardSelect, 'Chọn phường xã');
            fetchCoordinates(districtSelect.find(':selected').text() + ', ' + citySelect.find(':selected').text());
        }
        wardSelect.empty().append(`<option value="" disabled selected>Chọn phường xã</option>`);
        updateRegionLabel();
    });

    wardSelect.on('change', function () {
        fetchCoordinates(
            wardSelect.find(':selected').text() + ', ' +
            districtSelect.find(':selected').text() + ', ' +
            citySelect.find(':selected').text()
        );
        updateRegionLabel();
    });

    var latitude = parseFloat(latitudeInput.val()) || 16.047079; 
    var longitude = parseFloat(longitudeInput.val()) || 108.206230; 
    var map = L.map('map').setView([latitude, longitude], 15); 
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '© OpenStreetMap'
    }).addTo(map);

    var marker;

    function updateMap(lat, lon, label) {
        if (marker) {
            map.removeLayer(marker);
        }
        marker = L.marker([lat, lon]).addTo(map)
            .bindPopup(label)
            .openPopup();

        map.setView([lat, lon], 13);
    }

    map.on('click', function (e) {
        var lat = e.latlng.lat.toFixed(5);
        var lon = e.latlng.lng.toFixed(5);

        if (marker) {
            map.removeLayer(marker);
        }

        marker = L.marker([lat, lon]).addTo(map)
            .bindPopup(`Vị trí đã chọn: ${lat}, ${lon}`)
            .openPopup();

        latitudeInput.val(lat);
        longitudeInput.val(lon);
    });
});
