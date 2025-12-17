<template>
  <div class="relative h-screen w-full overflow-hidden">
    
    <div class="absolute inset-0 z-0">
      <swiper
        :modules="modules"
        :slides-per-view="1"
        :space-between="0"
        :loop="true"
        effect="fade"
        :autoplay="{ delay: 6000, disableOnInteraction: false }"
        class="h-full w-full"
      >
        <swiper-slide v-for="(img, index) in backgroundImages" :key="index">
          <img :src="img" class="h-full w-full object-cover" alt="Hastane Arka Plan" />
          <div class="absolute inset-0 bg-black/50"></div>
        </swiper-slide>
      </swiper>
    </div>

    <div class="relative z-10 flex flex-col justify-center items-center h-full">
        
        <img src="@/assets/vtys-logo.png" class="h-40 w-40 mb-4" alt="Logo">
        
        <div class="flex gap-2 bg-white/70 backdrop-blur-sm rounded-xl p-2 px-4">
            <span class="text-blue font-bold text-2xl drop-shadow-md">HASTANE</span>
            <span class="text-green font-bold text-2xl drop-shadow-md">YÖNETİM</span>
            <span class="text-red font-bold text-2xl drop-shadow-md">SİSTEMİ</span>
        </div>

        <div class="flex flex-col items-start mt-7">
            <div class="flex flex-row rounded-t cursor-pointer">
                <div 
                    @click="rolSec('Hasta')"
                    :class="[
                        'flex justify-center items-center h-10 w-20 text-white transition-colors duration-200 rounded-tl-lg font-bold',
                        secilenRol === 'Hasta' ? 'bg-blue' : 'bg-[#A6CBE7]'
                    ]"
                >
                    Hasta
                </div>
                <div 
                    @click="rolSec('Doktor')"
                    :class="[
                        'flex justify-center items-center h-10 w-20 text-white transition-colors duration-200 rounded-tr-lg font-bold',
                        secilenRol === 'Doktor' ? 'bg-blue' : 'bg-[#A6CBE7]'
                    ]"
                >
                    Doktor
                </div>
            </div>

            <div class="h-72 w-[500px] bg-white/50 backdrop-blur-sm rounded-xl px-10 py-5 rounded-tl-none shadow-2xl">
                <div class="flex flex-col">
                    <span class="font-semibold text-gray-700">Kullanıcı Adı:</span>
                    <input v-model="kullaniciAdi" type="text" class="bg-white h-9 rounded-lg mt-1 px-2 border focus:outline-none focus:border-blue-500">

                    <span class="mt-3 font-semibold text-gray-700">Şifre:</span>
                    <input v-model="sifre" type="password" class="bg-white h-9 rounded-lg mt-1 px-2 border focus:outline-none focus:border-blue-500">
                </div>

                <div 
                    @click="girisYap"
                    class="flex justify-center items-center h-10 w-56 bg-blue rounded-lg mt-9 cursor-pointer hover:bg-blue-700 text-white select-none transition duration-300 ease-in-out mx-auto shadow-lg"
                >
                    Giriş Yap
                </div>
                
                <p v-if="hataMesaji" class="text-red mt-2 text-center text-sm font-bold">
                    {{ hataMesaji }}
                </p>
            </div>
        </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import axios from "@/plugins/axios";
import { useRouter } from 'vue-router';

// --- Swiper Importları ---
import { Swiper, SwiperSlide } from 'swiper/vue';
import { Autoplay, EffectFade } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/effect-fade';

// --- Resim Importları ---
// Vite kullanıyorsan bu en garantili yoldur.
import bg1 from '@/assets/hospital1.jpg';
import bg2 from '@/assets/hospital2.jpg';
import bg3 from '@/assets/hospital3.jpg';

const router = useRouter();

// Swiper Modülleri ve Resim Listesi
const modules = [Autoplay, EffectFade];
const backgroundImages = [bg1, bg2, bg3];

// Değişkenler
const secilenRol = ref('Hasta');
const kullaniciAdi = ref('');
const sifre = ref('');
const hataMesaji = ref('');

// Rol Değiştirme Fonksiyonu
const rolSec = (rol) => {
    secilenRol.value = rol;
    hataMesaji.value = '';
};

// Giriş Yapma Fonksiyonu
const girisYap = async () => {
    if (!kullaniciAdi.value || !sifre.value) {
        hataMesaji.value = "Lütfen kullanıcı adı ve şifre giriniz.";
        return;
    }

    try {
        const response = await axios.post('http://localhost:5130/hastane/giris', {
            KullaniciAdi: kullaniciAdi.value,
            Sifre: sifre.value,
            Rol: secilenRol.value
        });

        if (response.status === 200) {
            console.log("Giriş Başarılı:", response.data);
            localStorage.setItem('user', JSON.stringify(response.data));

            if (secilenRol.value === 'Doktor') {
                router.push('/doctor');
            } else {
                router.push('/patient');
            }
        }

    } catch (error) {
        console.error(error);
        if (error.response && error.response.status === 401) {
            hataMesaji.value = "Kullanıcı adı veya şifre hatalı!";
        } else {
            hataMesaji.value = "Sunucuya bağlanılamadı.";
        }
    }
};
</script>