<template>
  <div>
    <h1>This route does not exits</h1>
  </div>
</template>

<script>
export default {
  mounted() {
    this.$router.push("/")
  },
}
</script>