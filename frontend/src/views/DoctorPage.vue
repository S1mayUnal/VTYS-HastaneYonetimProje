<template>
    <div class="px-20 py-14 bg-gray-50 min-h-screen relative">
        <Header class="mb-10"/>

        <div class="flex flex-row items-start justify-center w-full mt-20">
            <div class="flex flex-col shadow-lg z-10">
                <button 
                    @click="aktifTab = 1"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white rounded-tl-lg cursor-pointer', aktifTab === 1 ? 'bg-green' : 'bg-[#BACEA0] hover:bg-[#A8C285]']">
                    Randevu Listesi
                </button>
                
                <button 
                    @click="aktifTab = 2"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white cursor-pointer', aktifTab === 2 ? 'bg-green' : 'bg-[#9BBE6E] hover:bg-[#8AB65E]']">
                    Muayene Bilgileri
                </button>
                
                <button 
                    @click="aktifTab = 3"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white cursor-pointer', aktifTab === 3 ? 'bg-green' : 'bg-[#BACEA0] hover:bg-[#A8C285]']">
                    Hasta Takip
                </button>
                
                <button 
                    @click="aktifTab = 4"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white cursor-pointer', aktifTab === 4 ? 'bg-green' : 'bg-[#9BBE6E] hover:bg-[#8AB65E]']">
                    Rehber
                </button>

                <button 
                    @click="aktifTab = 5"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white rounded-bl-lg cursor-pointer', aktifTab === 5 ? 'bg-green' : 'bg-[#BACEA0] hover:bg-[#A8C285]']">
                    Nöbet Çizelgem
                </button>
            </div>

            <div class="bg-white w-[800px] min-h-[400px] px-10 pt-7 pb-14 rounded-lg rounded-tl-none shadow-xl border border-gray-100">
                
                <div v-if="aktifTab === 1">
                    <div v-if="yukleniyor" class="text-center mt-10 text-gray-500">Veriler yükleniyor...</div>
                    <div v-else-if="aktifRandevular.length === 0" class="text-center mt-10 p-5 bg-green-50 border border-green text-green-700 rounded-lg font-bold">
                        Bekleyen aktif randevunuz bulunmamaktadır.
                    </div>
                    <table v-else class="w-full text-left border-collapse">
                        <thead>
                            <tr class="bg-[#9BBE6E] text-white">
                                <th class="p-3 rounded-tl-lg">Tarih</th>
                                <th class="p-3">Saat</th>
                                <th class="p-3">Hasta Adı</th>
                                <th class="p-3 rounded-tr-lg text-right">İşlem</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="randevu in aktifRandevular" :key="randevu.randevu_id" class="border-b hover:bg-gray-50 transition-colors">
                                <td class="p-3 text-red font-bold">{{ tarihFormatla(randevu.randevu_tarihi) }}</td>
                                <td class="p-3 font-bold text-blue">{{ randevu.randevu_saati }}</td>
                                <td class="p-3 font-medium text-gray-700">{{ randevu.hastaAdSoyad }}</td>
                                <td class="p-3 text-right">
                                    <button 
                                        @click="muayeneModalAc(randevu)"
                                        class="bg-blue-500 text-white px-3 py-1 rounded text-sm hover:bg-blue-600 transition">
                                        Muayene Et
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div v-if="aktifTab === 2">
                    <div v-if="yukleniyor" class="text-center mt-10 text-gray-500">Veriler yükleniyor...</div>
                    <div v-else-if="gecmisRandevular.length === 0" class="text-center mt-10 p-5 bg-gray-50 border border-gray-200 text-gray-500 rounded-lg">
                        Geçmiş muayene kaydı bulunamadı.
                    </div>
                    <table v-else class="w-full text-left border-collapse">
                        <thead>
                            <tr class="bg-[#9BBE6E] text-white">
                                <th class="p-3 rounded-tl-lg">Tarih</th>
                                <th class="p-3">Hasta Adı</th>
                                <th class="p-3">Tanı</th>
                                <th class="p-3">Reçete</th>
                                <th class="p-3 rounded-tr-lg">Durum</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="randevu in gecmisRandevular" :key="randevu.randevu_id" class="border-b hover:bg-gray-50 transition-colors">
                                <td class="p-3 text-gray-600 text-sm">
                                    <div>{{ tarihFormatla(randevu.randevu_tarihi) }}</div>
                                    <div class="text-xs">{{ randevu.randevu_saati }}</div>
                                </td>
                                <td class="p-3 font-medium text-gray-800">{{ randevu.hastaAdSoyad }}</td>
                                <td class="p-3 italic text-gray-600 text-sm">{{ randevu.tani || '-' }}</td>
                                <td class="p-3">
                                    <div v-if="randevu.ilaclar && randevu.ilaclar.length > 0" class="text-xs text-blue-800 bg-blue-50 p-1 rounded border border-blue-100">
                                        <div v-for="(ilac, i) in randevu.ilaclar" :key="i">
                                            • {{ ilac.ilacAdi }}
                                        </div>
                                    </div>
                                    <span v-else class="text-xs text-gray-400">Yok</span>
                                </td>
                                <td class="p-3">
                                    <span :class="['px-2 py-1 rounded text-xs font-bold border', 
                                        randevu.randevu_durumu === 'Iptal' ? 'bg-red-50 text-red-700 border-red-200' : 'bg-gray-100 text-gray-600 border-gray-300']">
                                        {{ randevu.randevu_durumu }}
                                    </span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div v-if="aktifTab === 3">
                    <div v-if="yatanHastalar.length === 0" class="text-center mt-10 p-5 bg-green-50 border border-green text-green rounded-lg">
                        Şu anda serviste yatan hasta kaydı bulunmamaktadır.
                    </div>
                    <table v-else class="w-full text-left border-collapse">
                        <thead>
                            <tr class="bg-[#9BBE6E] text-white">
                                <th class="p-3 rounded-tl-lg">Hasta Adı</th>
                                <th class="p-3">Yattığı Oda</th>
                                <th class="p-3">Sorumlu Hemşire</th>
                                <th class="p-3">Giriş Tarihi</th>
                                <th class="p-3 rounded-tr-lg">Çıkış Tarihi</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="hasta in yatanHastalar" :key="hasta.yatis_id" class="border-b hover:bg-gray-50 transition-colors">
                                <td class="p-3 font-bold text-gray-700">{{ hasta.hastaAdSoyad }}</td>
                                <td class="p-3"><span class="bg-blue-100 text-blue-800 font-bold px-3 py-1 rounded-full text-sm">{{ hasta.odaNo || '-' }}</span></td>
                                <td class="p-3 text-sm text-gray-600">
                                    <div v-if="hasta.hemsireler && hasta.hemsireler.length > 0">
                                        <span v-for="(h, index) in hasta.hemsireler" :key="index" class="block">👩‍⚕️ {{ h }}</span>
                                    </div>
                                    <span v-else class="text-gray-400 italic">Atanmadı</span>
                                </td>
                                <td class="p-3 text-green-700 font-bold">{{ tarihFormatla(hasta.giris_tarihi) }}</td>
                                <td class="p-3 text-red-600 font-medium">{{ hasta.cikis_tarihi ? tarihFormatla(hasta.cikis_tarihi) : 'Taburcu Edilmedi' }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div v-if="aktifTab === 4">
                    <div class="flex flex-col gap-3">
                        <div v-for="dr in doktorlar" :key="dr.doktor_id" class="bg-gray-50 p-3 rounded border border-gray-200 hover:bg-blue-50 transition-colors">
                            <div class="flex flex-row items-center justify-between">
                                <span class="font-bold text-gray-800 text-lg">Dr. {{ dr.ad }} {{ dr.soyad }}</span>
                                <span class="bg-blue-100 text-blue-800 text-xs px-2 py-1 rounded-full font-bold">{{ dr.uzmanlik }}</span>
                            </div>
                            <div class="text-sm text-gray-600 mt-1 flex gap-4">
                                <span v-if="dr.telefon">📞 {{ dr.telefon }}</span>
                                <span v-if="dr.email">📧 {{ dr.email }}</span>
                            </div>
                        </div>
                    </div>
                </div>

                <div v-if="aktifTab === 5">
                    <div v-if="doktorNobetleri.length === 0" class="text-center mt-10 p-5 bg-gray-50 border border-dashed border-gray-300 text-gray-500 rounded-lg">
                        Yakın tarihte nöbetiniz bulunmamaktadır.
                    </div>
                    <table v-else class="w-full text-left border-collapse">
                        <thead>
                            <tr class="bg-[#9BBE6E] text-white">
                                <th class="p-3 rounded-tl-lg">Tarih</th>
                                <th class="p-3">Başlangıç</th>
                                <th class="p-3">Bitiş</th>
                                <th class="p-3 rounded-tr-lg">Nöbet Türü</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="nobet in doktorNobetleri" :key="nobet.baslangic" class="border-b hover:bg-gray-50">
                                <td class="p-3 font-bold text-gray-700">{{ tarihFormatla(nobet.baslangic) }}</td>
                                <td class="p-3 text-blue font-bold">{{ saatFormatla(nobet.baslangic) }}</td>
                                <td class="p-3 text-red font-bold">{{ saatFormatla(nobet.bitis) }}</td>
                                <td class="p-3">
                                    <span class="bg-yellow-100 text-yellow-800 px-2 py-1 rounded text-xs font-bold border border-yellow-200">{{ nobet.tur }}</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

            </div>
        </div>

        <div v-if="secilenRandevu" class="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div class="bg-white p-8 rounded-lg shadow-2xl w-[600px]">
                <h2 class="text-2xl font-bold text-gray-800 mb-6 border-b pb-2">Muayene Ekranı</h2>
                
                <div class="mb-4">
                    <label class="block text-gray-700 font-bold mb-2">Hasta:</label>
                    <div class="p-2 bg-gray-100 rounded text-gray-800">{{ secilenRandevu.hastaAdSoyad }}</div>
                </div>

                <div class="mb-4">
                    <label class="block text-gray-700 font-bold mb-2">Tanı (Teşhis):</label>
                    <textarea v-model="yeniTani" class="w-full border p-2 rounded h-24 focus:ring-2 ring-blue-300" placeholder="Hastanın şikayeti ve tanısı..."></textarea>
                </div>

                <div class="mb-6">
                    <label class="block text-gray-700 font-bold mb-2">İlaç Yaz (E-Reçete):</label>
                    <div class="flex gap-2 mb-2">
                        <select v-model="secilenIlacId" class="w-2/3 border p-2 rounded">
                            <option value="" disabled selected>İlaç Seçiniz</option>
                            <option v-for="ilac in ilacListesi" :key="ilac.ilac_id" :value="ilac.ilac_id">
                                {{ ilac.ilac_adi }} (Stok: {{ ilac.stok_adedi }})
                            </option>
                        </select>
                        <input v-model="kullanimSekli" type="text" class="w-1/3 border p-2 rounded" placeholder="Günde 2 tok vb.">
                    </div>
                </div>

                <div class="flex justify-end gap-3">
                    <button @click="secilenRandevu = null" class="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600 transition-colors">İptal</button>
                    <button @click="muayeneKaydet" class="bg-green-600 text-white px-6 py-2 rounded font-bold hover:bg-green-700 transition-colors">Kaydet ve Bitir</button>
                </div>
            </div>
        </div>

    </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import Header from '@/components/Header.vue';
import axios from '@/plugins/axios';

const aktifTab = ref(1);
const randevular = ref([]);
const doktorlar = ref([]); 
const yatanHastalar = ref([]);
const nobetler = ref([]);
const ilacListesi = ref([]); // YENİ: İlaç listesi
const yukleniyor = ref(true);
const girisYapanDoktor = ref(null);

// Modal State
const secilenRandevu = ref(null);
const yeniTani = ref("");
const secilenIlacId = ref("");
const kullanimSekli = ref("");

const tarihFormatla = (tarihString) => {
    if (!tarihString) return "";
    return new Date(tarihString).toLocaleDateString('tr-TR');
};

const saatFormatla = (tarihString) => {
    if (!tarihString) return "";
    return new Date(tarihString).toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
};

const verileriGetir = async () => {
    try {
        yukleniyor.value = true;
        const [randevuRes, doktorRes, yatanRes, nobetRes, ilacRes] = await Promise.all([
            axios.get('http://localhost:5130/hastane/randevular'),
            axios.get('http://localhost:5130/hastane/doktorlar'),
            axios.get('http://localhost:5130/hastane/yatan-hastalar'),
            axios.get('http://localhost:5130/hastane/nobetler'),
            axios.get('http://localhost:5130/hastane/ilaclar') // YENİ: İlaçları çek
        ]);

        randevular.value = randevuRes.data;
        doktorlar.value = doktorRes.data;
        yatanHastalar.value = yatanRes.data;
        nobetler.value = nobetRes.data;
        ilacListesi.value = ilacRes.data;
        
        yukleniyor.value = false;
    } catch (error) {
        console.error("Veri çekme hatası:", error);
        yukleniyor.value = false;
    }
};

const muayeneModalAc = (randevu) => {
    secilenRandevu.value = randevu;
    yeniTani.value = "";
    secilenIlacId.value = "";
    kullanimSekli.value = "";
};

const muayeneKaydet = async () => {
    if (!yeniTani.value) {
        alert("Lütfen bir tanı giriniz.");
        return;
    }

    try {
        // 1. Tanıyı Kaydet
        await axios.post('http://localhost:5130/hastane/muayene-yap', {
            randevu_id: secilenRandevu.value.randevu_id,
            tani: yeniTani.value
        });

        // 2. İlaç Seçildiyse Reçeteye Ekle
        if (secilenIlacId.value) {
            await axios.post('http://localhost:5130/hastane/recete-ekle', {
                randevu_id: secilenRandevu.value.randevu_id,
                ilac_id: secilenIlacId.value,
                kullanim_sekli: kullanimSekli.value || "Tarif edilmedi",
                adet: 1
            });
        }

        alert("Muayene tamamlandı.");
        secilenRandevu.value = null; // Modalı kapat
        await verileriGetir(); // Listeyi güncelle
        aktifTab.value = 2; // Geçmiş (Tamamlanan) sekmesine at

    } catch (error) {
        alert("Hata: " + (error.response?.data?.message || error.message));
    }
};

onMounted(() => {
    const userStr = localStorage.getItem('user');
    if (userStr) {
        girisYapanDoktor.value = JSON.parse(userStr);
    }
    verileriGetir();
});

// --- COMPUTED ---

// 1. Bekleyen (Aktif) Randevular
const aktifRandevular = computed(() => {
    if (!girisYapanDoktor.value) return [];
    return randevular.value.filter(r => 
        r.doktor_id === girisYapanDoktor.value.doktor_id && 
        r.randevu_durumu === 'Aktif'
    );
});

// 2. Geçmiş (Tamamlanan/İptal) Randevular
const gecmisRandevular = computed(() => {
    if (!girisYapanDoktor.value) return [];
    return randevular.value.filter(r => 
        r.doktor_id === girisYapanDoktor.value.doktor_id && 
        r.randevu_durumu !== 'Aktif'
    );
});

// 3. Nöbetler
const doktorNobetleri = computed(() => {
    if (!girisYapanDoktor.value) return [];
    return nobetler.value.filter(n => n.doktor_id === girisYapanDoktor.value.doktor_id);
});
</script>