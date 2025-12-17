import axios from 'axios'

const instance = axios.create({
    baseURL: 'http://localhost:5130/api', 
    timeout: 10000, // 10 saniye içinde cevap gelmezse iptal et
    headers: {
        'Content-Type': 'application/json'
    }
});
export default instance;
//kullanım
//const res = await api.get('/test')
//console.log(res.data.message)
