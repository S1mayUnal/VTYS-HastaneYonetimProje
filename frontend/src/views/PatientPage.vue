<template>
    <div class="px-20 py-14 bg-gray-50 min-h-screen">
        <Header class="mb-10"/>

        <div class="flex flex-row items-start justify-center w-full mt-20">
            <div class="flex flex-col shadow-lg z-10">
                <button 
                    @click="aktifTab = 1"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white rounded-tl-lg cursor-pointer', aktifTab === 1 ? 'bg-blue' : 'bg-[#A6CBE7]']">
                    Randevu Al
                </button>
                
                <button 
                    @click="aktifTab = 2"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white cursor-pointer', aktifTab === 2 ? 'bg-blue' : 'bg-[#9CC4E2]']">
                    Randevularım
                </button>
                
                <button 
                    @click="aktifTab = 3"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white cursor-pointer', aktifTab === 3 ? 'bg-blue' : 'bg-[#A6CBE7]']">
                    Geçmiş Randevular
                </button>

                <button 
                    @click="aktifTab = 4"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white cursor-pointer', aktifTab === 4 ? 'bg-blue' : 'bg-[#9CC4E2]']">
                    Laboratuvar Sonuçlarım
                </button>

                <button 
                    @click="aktifTab = 5"
                    :class="['h-12 w-[220px] text-center font-bold transition-colors text-white rounded-bl-lg cursor-pointer', aktifTab === 5 ? 'bg-blue' : 'bg-[#A6CBE7]']">
                    Faturalarım
                </button>
            </div>

            <div class="bg-white w-[800px] min-h-[400px] px-10 pt-7 pb-14 rounded-lg rounded-tl-none shadow-xl border border-gray-100 relative">
                
                <div v-if="aktifTab === 1">
                    <span class="text-blue font-semibold block mb-1">Klinik:</span>
                    <select 
                        v-model="secilenUzmanlik"
                        @change="klinikDegisti"
                        class="w-full h-10 border border-gray-300 rounded-md mb-4 px-3 focus:outline-none focus:border-blue transition-colors bg-white">
                        <option value="" disabled selected>Klinik Seçiniz</option>
                        <option v-for="uzmanlik in klinikler" :key="uzmanlik" :value="uzmanlik">
                            {{ uzmanlik }}
                        </option>
                    </select>
                    
                    <span class="text-blue font-semibold block mb-1">Hekim:</span>
                    <select 
                        v-model="secilenDoktor"
                        :disabled="!secilenUzmanlik"
                        class="w-full h-10 border border-gray-300 rounded-md mb-4 px-3 focus:outline-none focus:border-blue transition-colors bg-white disabled:bg-gray-100 disabled:text-gray-400">
                        <option value="" disabled selected>
                            {{ secilenUzmanlik ? 'Doktor Seçiniz' : 'Önce Klinik Seçiniz' }}
                        </option>
                        <option v-for="doktor in filtrelenenDoktorlar" :key="doktor.doktor_id" :value="doktor.doktor_id">
                            {{ doktor.ad }} {{ doktor.soyad }}
                        </option>
                    </select>
                    
                    <span class="text-blue font-semibold block mb-1">Tarih:</span>
                    <input type="date" v-model="secilenTarih" class="w-full h-10 border border-gray-300 rounded-md mb-4 px-3 focus:outline-none focus:border-blue-500 transition-colors">
                    
                    <div v-if="secilenDoktor && secilenTarih && nobetKontrol(secilenDoktor, secilenTarih)" 
                         class="bg-green-100 text-green-700 p-3 rounded-md mb-4 text-sm font-semibold border border-green-200">
                        ✓ Bu doktor seçilen tarihte NÖBETÇİ doktordur.
                    </div>

                    <button 
                        @click="randevuOlustur"
                        class="bg-red hover:bg-red-600 h-10 w-32 rounded-md text-center text-white mt-4 font-bold transition-colors shadow-md cursor-pointer">
                        Onayla
                    </button>
                </div>

                <div v-if="aktifTab === 2">
                    <div v-if="yukleniyor" class="text-center py-10 text-gray-500">Veriler yükleniyor...</div>
                    <div v-else-if="hastaAktifRandevulari.length === 0" class="text-center py-10 bg-blue-50 rounded-lg border border-dashed border-blue-200 text-blue-800">
                        <p>Henüz aktif bir randevunuz bulunmuyor.</p>
                    </div>
                    <div v-else class="overflow-hidden rounded-lg border border-gray-200 shadow-sm">
                        <table class="w-full text-left border-collapse bg-white">
                            <thead class="bg-blue-50">
                                <tr>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Tarih</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Klinik</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Doktor</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800 text-right">Durum</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-100">
                                <tr v-for="randevu in hastaAktifRandevulari" :key="randevu.randevu_id" class="hover:bg-gray-50">
                                    <td class="p-4 text-red font-bold">{{ tarihFormatla(randevu.randevu_tarihi) }}</td>
                                    <td class="p-4"><span class="bg-blue-100 text-blue-800 text-xs px-2 py-1 rounded-full">{{ randevu.bolum }}</span></td>
                                    <td class="p-4 text-gray-700">Dr. {{ randevu.doktorAdSoyad }}</td>
                                    <td class="p-4 text-right"><span class="text-blue-600 font-bold text-sm">{{ randevu.randevu_durumu }}</span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div v-if="aktifTab === 3">
                    <div v-if="hastaGecmisRandevulari.length === 0" class="text-center py-10 bg-gray-50 rounded-lg border border-dashed border-red text-red">
                        <p>Geçmiş randevu kaydı bulunamadı.</p>
                    </div>
                    <div v-else class="overflow-hidden rounded-lg border border-gray-200 shadow-sm">
                        <table class="w-full text-left border-collapse bg-white">
                            <thead class="bg-blue-50">
                                <tr>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Tarih</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Klinik & Doktor</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Tanı</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800 text-right">İşlem</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-100">
                                <tr v-for="randevu in hastaGecmisRandevulari" :key="randevu.randevu_id" class="hover:bg-gray-50">
                                    <td class="p-4 text-red font-bold">{{ tarihFormatla(randevu.randevu_tarihi) }}</td>
                                    <td class="p-4">
                                        <div class="font-bold text-gray-800">{{ randevu.bolum }}</div>
                                        <div class="text-xs text-gray-500">Dr. {{ randevu.doktorAdSoyad }}</div>
                                    </td>
                                    <td class="p-4 text-sm text-gray-600 italic">{{ randevu.tani || '-' }}</td>
                                    <td class="p-4 text-right">
                                        <button 
                                            v-if="randevu.ilaclar && randevu.ilaclar.length > 0"
                                            @click="receteGoster(randevu)"
                                            class="bg-blue hover:bg-blue-700 text-white text-xs px-3 py-1 rounded transition-colors">
                                            Reçete Gör
                                        </button>
                                        <span v-else class="text-xs text-gray-400">Reçete Yok</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div v-if="aktifTab === 4">
                    <div v-if="tahliller.length === 0" class="text-center py-10 bg-gray-50 rounded-lg border border-dashed border-purple-300 text-purple-700">
                        <p>Sisteme kayıtlı laboratuvar sonucunuz bulunmamaktadır.</p>
                    </div>
                    <div v-else class="overflow-hidden rounded-lg border border-gray-200 shadow-sm">
                        <table class="w-full text-left border-collapse bg-white">
                            <thead class="bg-blue-50">
                                <tr>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Tarih</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">İşlem Adı</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Sonuç</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800 text-right">Durum</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-100">
                                <tr v-for="tahlil in tahliller" :key="tahlil.id" class="hover:bg-gray-50">
                                    <td class="p-4 text-red font-bold">{{ tarihFormatla(tahlil.tarih) }}</td>
                                    <td class="p-4 font-bold text-gray-800">{{ tahlil.tahlilAdi }}</td>
                                    <td class="p-4 text-sm text-gray-700">
                                        <div class="max-w-[200px] truncate" :title="tahlil.sonuc">
                                            {{ tahlil.sonuc || 'Sonuç Bekleniyor' }}
                                        </div>
                                    </td>
                                    <td class="p-4 text-right">
                                        <span :class="['px-2 py-1 rounded text-xs font-bold border', 
                                            tahlil.sonuc ? 'bg-green-100 text-green-700 border-green-200' : 'bg-yellow-50 text-yellow-700 border-yellow-200']">
                                            {{ tahlil.sonuc ? 'Sonuçlandı' : 'Hazırlanıyor' }}
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div v-if="aktifTab === 5">
                    <div v-if="faturalar.length === 0" class="text-center py-10 bg-gray-50 rounded-lg border border-dashed border-gray-300 text-gray-500">
                        <p>Kayıtlı faturanız bulunmamaktadır.</p>
                    </div>
                    <div v-else class="overflow-hidden rounded-lg border border-gray-200 shadow-sm">
                        <table class="w-full text-left border-collapse bg-white">
                            <thead class="bg-blue-50">
                                <tr>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Tarih</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Hizmet</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800">Tutar</th>
                                    <th class="p-4 text-sm font-semibold text-blue-800 text-right">Durum</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-100">
                                <tr v-for="fatura in faturalar" :key="fatura.fatura_id" class="hover:bg-gray-50">
                                    <td class="p-4 text-red font-bold">{{ tarihFormatla(fatura.fatura_tarihi) }}</td>
                                    <td class="p-4 text-sm font-bold">{{ fatura.kaynak }}</td>
                                    <td class="p-4 text-green font-bold">{{ fatura.son_odeme_tutari }} TL</td>
                                    <td class="p-4 text-right">
                                        <span :class="['px-2 py-1 rounded text-xs font-bold border', 
                                            fatura.odeme_durumu === 'Odendi' ? 'bg-green-100 text-green-700 border-green-200' : 'bg-yellow-50 text-yellow-700 border-yellow-200']">
                                            {{ fatura.odeme_durumu }}
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div v-if="seciliReceteRandevusu" class="absolute inset-0 bg-[#D3E9F6] bg-opacity-30 flex items-center justify-center rounded-tr-lg rounded-br-lg rounded-bl-lg z-50">
                    <div class="bg-white p-6 rounded-lg shadow-2xl w-[90%] max-w-md">
                        <h3 class="text-lg font-bold text-blue mb-4 border-b pb-2">Reçete Detayı</h3>
                        <div class="space-y-3 mb-6 max-h-60 overflow-y-auto">
                            <div v-for="(ilac, index) in seciliReceteRandevusu.ilaclar" :key="index" class="bg-gray-50 p-3 rounded border border-gray-200">
                                <div class="font-bold text-gray-800">{{ ilac.ilacAdi }}</div>
                                <div class="text-sm text-gray-600 flex justify-between mt-1">
                                    <span>{{ ilac.kullanim }}</span>
                                    <span class="bg-blue-100 text-blue-800 px-2 rounded-full text-xs">x{{ ilac.adet }}</span>
                                </div>
                            </div>
                        </div>
                        <button @click="seciliReceteRandevusu = null" class="w-full bg-red text-white py-2 rounded font-bold hover:bg-red-600">Kapat</button>
                    </div>
                </div>

            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import Header from '@/components/Header.vue';
import axios from '@/plugins/axios';

// --- STATE ---
const aktifTab = ref(2); 
const randevular = ref([]);
const doktorlar = ref([]); 
const faturalar = ref([]); 
const tahliller = ref([]); // YENİ: Tahlil sonuçları için
const nobetler = ref([]); 
const girisYapanKullanici = ref(null);
const yukleniyor = ref(true);

// Form Değişkenleri
const secilenUzmanlik = ref("");
const secilenDoktor = ref("");
const secilenTarih = ref("");
const seciliReceteRandevusu = ref(null); 

// --- COMPUTED ---

// 1. Klinikler
const klinikler = computed(() => {
    if (doktorlar.value.length === 0) return [];
    const uzmanliklar = doktorlar.value.map(d => d.uzmanlik); 
    return [...new Set(uzmanliklar)].sort();
});

// 2. Doktor Filtreleme
const filtrelenenDoktorlar = computed(() => {
    if (!secilenUzmanlik.value) return [];
    return doktorlar.value.filter(doktor => doktor.uzmanlik === secilenUzmanlik.value);
});

// 3. Aktif Randevular
const hastaAktifRandevulari = computed(() => {
    if (!girisYapanKullanici.value) return []; 
    return randevular.value.filter(r => r.hasta_id === girisYapanKullanici.value.hasta_id && r.randevu_durumu === 'Aktif');
});

// 4. Geçmiş Randevular
const hastaGecmisRandevulari = computed(() => {
    if (!girisYapanKullanici.value) return []; 
    return randevular.value.filter(r => r.hasta_id === girisYapanKullanici.value.hasta_id && r.randevu_durumu !== 'Aktif');
});

// --- FONKSİYONLAR ---

const nobetKontrol = (doktorId, tarih) => {
    if (!nobetler.value.length) return false;
    return nobetler.value.some(n => 
        n.doktor_id === doktorId && 
        n.baslangic.startsWith(tarih)
    );
};

const tarihFormatla = (tarihString) => {
    if (!tarihString) return "";
    return new Date(tarihString).toLocaleDateString('tr-TR');
};

const receteGoster = (randevu) => {
    seciliReceteRandevusu.value = randevu;
};

const verileriGetir = async () => {
    try {
        yukleniyor.value = true;
        
        // 1. Randevular
        const randevuRes = await axios.get('http://localhost:5130/hastane/randevular');
        randevular.value = randevuRes.data;

        // 2. Doktorlar
        const doktorRes = await axios.get('http://localhost:5130/hastane/doktorlar');
        doktorlar.value = doktorRes.data;

        // 3. Nöbetler
        const nobetRes = await axios.get('http://localhost:5130/hastane/nobetler');
        nobetler.value = nobetRes.data;

        if (girisYapanKullanici.value) {
            // 4. Faturalar
            const faturaRes = await axios.get(`http://localhost:5130/hastane/faturalar/${girisYapanKullanici.value.hasta_id}`);
            faturalar.value = faturaRes.data;

            // 5. Tahliller (YENİ)
            // Backend tarafında 'tahliller' endpointi henüz yoksa burası 404 verebilir veya boş dönebilir.
            // Şimdilik try-catch ile sararak hata verse bile sayfanın açılmasını sağlıyoruz.
            try {
                const tahlilRes = await axios.get(`http://localhost:5130/hastane/tahliller/${girisYapanKullanici.value.hasta_id}`);
                tahliller.value = tahlilRes.data;
            } catch (err) {
                console.warn("Tahliller çekilemedi (Backend endpoint eksik olabilir).");
                tahliller.value = [];
            }
        }

        yukleniyor.value = false;
    } catch (error) {
        console.error("Veri çekme hatası:", error);
        yukleniyor.value = false;
    }
};

const klinikDegisti = () => {
    secilenDoktor.value = "";
};

const randevuOlustur = async () => {
    if (!secilenUzmanlik.value || !secilenDoktor.value || !secilenTarih.value) {
        alert("Lütfen tüm alanları doldurunuz.");
        return;
    }

    try {
        yukleniyor.value = true;
        const yeniRandevu = {
            doktor_id: secilenDoktor.value, 
            randevu_tarihi: secilenTarih.value,
            hasta_id: girisYapanKullanici.value.hasta_id
        };

        await axios.post('http://localhost:5130/hastane/randevu-olustur', yeniRandevu);
        alert("Randevunuz oluşturuldu!");
        
        secilenUzmanlik.value = "";
        secilenDoktor.value = "";
        secilenTarih.value = "";
        
        await verileriGetir();
        aktifTab.value = 2;

    } catch (error) {
        alert("Hata: " + (error.response?.data?.error || error.message));
    } finally {
        yukleniyor.value = false;
    }
};

onMounted(() => {
    const userStr = localStorage.getItem('user');
    if (userStr) {
        girisYapanKullanici.value = JSON.parse(userStr);
    }
    verileriGetir();
});
</script>