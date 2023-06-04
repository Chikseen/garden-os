<template>
  <Overview v-if="isAuth" />
  <Landing v-else />
</template>

<script>
import Overview from "@/views/Overview.vue";
import Landing from "@/views/Landing.vue";

import { mapState } from "vuex";

export default {
  name: "Index",
  components: {
    Overview,
    Landing,
  },
  computed: {
    ...mapState({
      isAuth: (state) => state.isAuth,
    }),
  },
};
</script>