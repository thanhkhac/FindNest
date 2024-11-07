function previewImages(inputFiles, displayTarget, indexInputName, indexClassName) {
    const files = inputFiles.files;

    $.each(files, function (index, file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const imgContainer = $('<div class="sortable-img position-relative"></div>');
            const img = $('<img>').attr('src', e.target.result).addClass('h-100 rounded');

            const fileInput = $('<input>').attr({
                type: 'file',
                name: inputFiles.name, 
                style: 'display: none;' 
            });

            const dataTransfer = new DataTransfer();
            dataTransfer.items.add(file);
            fileInput[0].files = dataTransfer.files; 

            const indexInput = $('<input>').attr({
                type: 'hidden',
                name: indexInputName, 
                value: '', 
                class: 'FileIndex'
            });

            const removeBtn = $('<button class="btn btn-dark opacity-75 rounded-circle position-absolute py-0 px-2 top-0 end-0">x</button>');

            removeBtn.on('click', function () {
                imgContainer.remove(); 
                updateIndexInputs(); 
            });

            imgContainer.append(img).append(removeBtn).append(fileInput).append(indexInput); 
            $(displayTarget).append(imgContainer); 

            updateIndexInputs();
        };
        reader.readAsDataURL(file);
    });

    $(displayTarget).sortable({
        items: '.sortable-img', 
        cursor: 'move', 
        placeholder: 'sortable-placeholder', 
        update: function (event, ui) {
            updateIndexInputs();
        }
    });

    function updateIndexInputs() {
        $(displayTarget).children().each(function () {
            const index = $(this).index();
            console.log(indexInputName)
            console.log(index)
            $(this).find('input[type="hidden"].' + indexClassName).val(index);
        });
    }
}

