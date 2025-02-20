// Şehir verileri (Trabzon yerine Eskişehir eklendi)
const sehirler = ["İstanbul", "Ankara", "İzmir", "Antalya", "Bursa", "Adana", "Eskişehir"];

// Şehirleri alfabetik sıraya göre sıralama
sehirler.sort();

// Fonksiyon: Alış ve Teslim ofisi dropdown'larını doldur
function fillSehirDropdown(alisId, teslimId) {
    const alisOfisiDropdown = document.getElementById(alisId);
    const teslimOfisiDropdown = document.getElementById(teslimId);

    // Şehirleri dropdown'lara ekle
    sehirler.forEach(sehir => {
        const alisOption = document.createElement("option");
        alisOption.value = sehir;
        alisOption.textContent = sehir;
        alisOfisiDropdown.appendChild(alisOption);

        const teslimOption = alisOption.cloneNode(true);
        teslimOfisiDropdown.appendChild(teslimOption);
    });
}

// İlk alan için şehirleri doldur
fillSehirDropdown("alisOfisi1", "teslimOfisi1");

// İkinci alan için şehirleri doldur
fillSehirDropdown("alisOfisi2", "teslimOfisi2");
