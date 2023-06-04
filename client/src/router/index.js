import { createRouter, createWebHistory } from "vue-router";
import DeviceDetailedView from "@/views/DeviceDetailedView";
import NotFound from "@/views/NotFound";
import Index from "@/views/Index";

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
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
