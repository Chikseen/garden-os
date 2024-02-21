import { createRouter, createWebHistory } from "vue-router";
import DeviceDetailedView from "@/views/DeviceDetailedView";
import NotFound from "@/views/NotFound";
import Index from "@/views/Index";
import Overview from "@/views/Overview";
import GardenOverview from "@/views/GardenOverview";
import UserOverview from "@/views/UserOverview";
import HubLog from "@/views/HubLog";
import DeviceSetting from "@/views/DeviceSettingView";


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
    path: '/garden',
    component: GardenOverview,
  },
  {
    path: '/user',
    component: UserOverview,
  },
  {
    path: '/hublog',
    component: HubLog,
  },
  {
    path: '/devicesetting',
    component: DeviceSetting,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
