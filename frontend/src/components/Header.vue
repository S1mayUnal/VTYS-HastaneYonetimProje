<template>
    <div class="flex flex-row justify-between">
        <router-link to="/" class="flex flex-row items-center">
            <img src="@/assets/vtys-logo.png" class="h-16 w-16 mr-3" alt="logo">
            <span class="text-blue font-bold text-xl mr-2">HASTANE</span>
            <span class="text-green font-bold text-xl mr-2">YÖNETİM</span>
            <span class="text-red font-bold text-xl">SİSTEMİ</span>
        </router-link>
        
        <div class="bg-red h-7 w-56 rounded-md text-white mt-4
            flex items-center justify-center gap-2">
    <img src="@/assets/user.svg" alt="user" class="h-4 w-4">
    {{ kullaniciAdSoyad }}
</div>

    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const kullaniciAdSoyad = ref('');

onMounted(() => {
    const userStr = localStorage.getItem('user');
    
    if (userStr) {
        try {
            const user = JSON.parse(userStr);
            
            if (user && user.ad && user.soyad) {
                // Kullanıcının doktor olup olmadığını kontrol ediyoruz.
                // Veritabanındaki alan adına göre (rol, tur, unvan vb.) burayı düzenleyebilirsin.
                // Örnek olarak 'rol' veya 'tur' alanının 'Doktor' olup olmadığına bakıyoruz.
                let unvan = '';
                if (user.rol === 'Doktor' || user.tur === 'Doktor' || user.unvan === 'Doktor') {
                    unvan = 'Dr. ';
                }
                
                kullaniciAdSoyad.value = `${unvan}${user.ad} ${user.soyad}`;
            }
        } catch (e) {
            console.error("Kullanıcı verisi okunurken hata:", e);
        }
    }
});
</script>