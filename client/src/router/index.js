import { createRouter, createWebHistory } from "vue-router";
import DeviceDetailedView from "@/views/DeviceDetailedView";
import NotFound from "@/views/NotFound";
import Index from "@/views/Index";
import Login from "@/views/Login";
import Overview from "@/views/Overview";
import GardenOverview from "@/views/GardenOverview";
import HubLog from "@/views/HubLog";


const routes = [
  {
    path: "/",
    name: "home",
    component: Index,
  },
  {
    path: "/device/:id",
    name: "deviceDetailed",
    component: DeviceDetailedView,
  },
  {
    path: '/:pathMatch(.*)*',
    component: NotFound,
  },
  {
    path: '/overview',
    component: Overview,
  },
  {
    path: '/login',
    component: Login,
  },
  {
    path: '/garden',
    component: GardenOverview,
  },
  {
    path: '/hublog',
    component: HubLog,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
