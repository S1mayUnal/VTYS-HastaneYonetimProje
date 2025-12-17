import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
import DoctorPage from "@/views/DoctorPage.vue";
import PatientPage from "@/views/PatientPage.vue";


const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
    meta: {
      title: "E-Autotrek - Ana Sayfa",
      description: "E-Autotrek Ana Sayfası",
    },
  },
  {
    path: "/doctor",
    name: "DoctorPage",
    component: DoctorPage,
  },
  {
    path: "/patient",
    name: "PatientPage",
    component: PatientPage,
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    } else {
      return { top: 0, left: 0, behavior: "smooth" };
    }
  },
});

export default router;
