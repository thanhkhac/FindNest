function previewImage(inputFiles, displayTarget) {
    const files = inputFiles.files;
    if (files.length === 0) return; 

    const reader = new FileReader();
    $(displayTarget).empty(); 

    reader.onload = function (e) {
        const img = $('<img>').attr('src', e.target.result).attr('style', 'object-fit:cover').addClass('w-100');
        $(displayTarget).append(img);
    };

    reader.readAsDataURL(files[0]); 
}