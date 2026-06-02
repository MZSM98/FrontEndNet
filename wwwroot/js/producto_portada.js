const imagen = document.querySelector(".portada");
const archivo = document.getElementById("ArchivoId");

function CargaImagen() {
    if (archivo.selectedIndex > 0) {
        const path = imagen.dataset.url + "/" + archivo.options[archivo.selectedIndex].value;
        imagen.src = path;
    } else {
        // Nueva URL segura
        imagen.src = "https://dummyimage.com/300x450/cccccc/000000.png&text=Sin+Portada";
    }
}

archivo.addEventListener("change", CargaImagen);