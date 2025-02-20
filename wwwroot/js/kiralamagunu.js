// Tarih inputları seçildiğinde kiralama süresi hesaplanacak
document.getElementById("alisTarihi").addEventListener("change", calculateDays);
document.getElementById("teslimTarihi").addEventListener("change", calculateDays);

function calculateDays() {
    // Alış ve teslim tarihlerini al
    const alisTarihi = new Date(document.getElementById("alisTarihi").value);
    const teslimTarihi = new Date(document.getElementById("teslimTarihi").value);

    // Tarihler arasındaki farkı gün olarak hesapla
    if (alisTarihi && teslimTarihi && teslimTarihi > alisTarihi) {
        const fark = Math.ceil((teslimTarihi - alisTarihi) / (1000 * 60 * 60 * 24)); // milisaniye cinsinden farkı alıp gün cinsine dönüştür
        document.getElementById("kiralamaSuresi").style.display = "block"; // "Kaç gün kiralandı?" kısmını göster
        document.getElementById("günSayisi").innerText = fark; // Hesaplanan günü ekrana yazdır
    } else {
        // Eğer tarih geçersizse, kiralama süresi görünmesin
        document.getElementById("kiralamaSuresi").style.display = "none";
    }
}

// Kirala butonuna tıklanması durumunda herhangi bir işlem yapılmaz
document.getElementById("rentButton").addEventListener("click", function(event) {
    event.preventDefault();  // Formun submit olmasını engeller
});
