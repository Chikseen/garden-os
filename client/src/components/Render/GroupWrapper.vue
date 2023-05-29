<template>
  <Group v-for="(object, index) in objects" :key="index" :position="object.position">
    <GroupWrapper v-if="object.type === 'group'" :objects="object.childs"></GroupWrapper>
    <Box v-else-if="object.type === 'box'" :position="object.position" :scale="object.scale">
      <PhongMaterial :color="object.color" />
    </Box>
    <Box v-else-if="object.type === 'box'" :position="object.position" :scale="object.scale">
      <PhongMaterial :color="object.color" />
    </Box>
    <GltfModel v-else-if="object.type === 'gltf'" :src="object.src" :position="object.position" :scale="object.scale" />
  </Group>
</template>

<script>
export default {
  data() {
    return {
    }
  },
  props: {
    objects: { type: Array, default: ()=>[] }
  }
}
</script>
