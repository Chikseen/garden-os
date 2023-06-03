<template>
  <div class="render_wrapper">
    <Renderer ref="rendererC" class="render" antialias resize :orbit-ctrl="{ enableDamping: true, dampingFactor: 0.05 }"
      pointer shadow>
      <Camera ref="camera" :position="{ z: 30, x: 0, y: 0 }" :fov="80" />
      <Scene ref="scene" background="#2b2b2b">
        <PointLight :position="{ y: 100, z: 100, x: 100 }" :intensity="1" :cast-shadow="castShadow"
          :shadow-map-size="{ width: 2048, height: 2048 }" />
        <AmbientLight color="#aaaaaa" :intensity="0.05" />

        <GroupWrapper :objects="mapData?.objects"></GroupWrapper>

        <div>
          <!-- <BoxFrame ref="boxframe" :position="{ x: -100, y: -210 }" :boxScale="{ x: 50, y: 50, z: 50 }" :setName="item" :text="'PP'" />-->
        </div>
      </Scene>
      <div class="editor">
        <textarea :value="JSON.stringify(mapData, null, 2)" @change="setNewJSON" rows="10" cols="70"></textarea>
      </div>
      <div class="exit" @click="$store.commit('set3dView', false)">
        <h3>EXIT 3D VIEW</h3>
      </div>
    </Renderer>
  </div>
</template>

<script>
import { Raycaster, Vector2 } from "three-full";
import GroupWrapper from "@/components/Render/GroupWrapper.vue";

let raycaster = new Raycaster();
let pointer = new Vector2();
let allChilds = [];


export default {
  components: { GroupWrapper },
  props: {},
  data() {
    return {
      mapData: null,
      castShadow: false,

      yoffset: 0,
      xoffset: 0,
      zoffset: 1,
      yscale: 1,
      xscale: 1,
      zscale: 1,
      mouseDown: { x: 0, y: 0 },
      main_selected: 0,
      scale_selected: 0,
      level_selected: 0,

      projectData: {
        main: {
          pcr: [

          ]
        }
      },

    };
  },
  methods: {
    async setNewJSON(e) {
      try {
        const requestData = JSON.parse(e.target.value)
        const initData = await fetch(`${process.env.VUE_APP_PI_HOST}maps/${"035e762f-02aa-422e-9864-51c0056f5804"}`, {
          method: "POST",
          body: JSON.stringify({ "json": JSON.stringify(requestData) }),
          headers: {
            'Authorization': `Bearer ${localStorage.getItem("apiToken")}`,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
        });
        const res = await initData.json()
        console.log(res)
        this.mapData = JSON.parse(res.json);

      } catch (error) {
        console.log("Invalid JSON File", error)
      }
    },


    getcurrentdata() {
      console.log("get box data", this.$refs.boxframe);
    },
    onPointerEvent() {
      console.log("hi");
    },
    getAllChilds(group) {
      group.forEach((child) => {
        if (child.type === "Group") {
          this.getAllChilds(child.children);
        } else {
          allChilds.push(child);
        }
      });
    },
    saveMouseDown(event) {
      this.mouseDown.x = event.clientX;
      this.mouseDown.y = event.clientY;
    },
    click(event) {
      if (this.mouseDown.x == event.clientX && this.mouseDown.y == event.clientY) {
        pointer.x = (event.clientX / window.innerWidth) * 2 - 1;
        pointer.y = -(event.clientY / window.innerHeight) * 2 + 1;

        raycaster.setFromCamera(pointer, this.$refs.camera.camera);

        allChilds = [];
        if (this.$refs.scene.scene.children.length > 0) {
          this.getAllChilds(this.$refs.scene.scene.children);
        }
        const intersects = raycaster.intersectObjects(allChilds);
        const filterMesh = intersects.filter((element) => this.projectData.main.pcr[element.object.name]);
        const result = filterMesh.filter((element) => this.projectData.main.pcr[element.object.name].level == this.level_selected);

        if (event.clientX > 240 || event.clientY > 270) {
          if (result[0]) {
            const box = result[0];
            switch (parseInt(this.main_selected)) {
              case 0: {
                this.changePostion(box);
                break;
              }
              case 1: {
                this.changeScale(box);
                break;
              }
              case 2: {
                console.log("not supported yet");
                break;
              }
            }
          } else {
            console.log("sometginh went wrong in the box replacement proccess");
          }
        } else {
          console.log("box click");
        }
      } else {
        console.log("mouse movment dont move block");
      }
    },
  },
  async mounted() {
    const initData = await fetch(`${process.env.VUE_APP_PI_HOST}maps/${"035e762f-02aa-422e-9864-51c0056f5804"}`, {
      method: "GET",
      headers: {
        'Authorization': `Bearer ${localStorage.getItem("apiToken")}`,
      },
    });

    const res = await initData.json()
    console.log(res)
    this.mapData = JSON.parse(res.json);

    const r = this.$refs.rendererC
    r.renderer.setPixelRatio(window.devicePixelRatio);

    window.addEventListener("pointermove", (event) => {
      if (this.$refs.camera) {
        if (this.main_selected != 2) {
          pointer.x = (event.clientX / window.innerWidth) * 2 - 1;
          pointer.y = -(event.clientY / window.innerHeight) * 2 + 1;

          raycaster.setFromCamera(pointer, this.$refs.camera.camera);

          allChilds = [];

          if (this.$refs.scene.scene.children.length > 0) {
            this.getAllChilds(this.$refs.scene.scene.children);
          }

          allChilds.forEach((element) => {
            if (element.type == "Mesh") {
              if (element.name != "") {
                if (this.projectData.main.pcr[element.name]) {
                  if (this.projectData.main.pcr[element.name].level == this.level_selected) {
                    element.material.color.set(0xa5df6e);
                    element.material.opacity = 0.4;
                  } else {
                    element.material.color.set(0xa3a5a1);
                    element.material.opacity = 0.1;
                  }
                }
              }
            }
          });

          const intersects = raycaster.intersectObjects(allChilds);
          const filterMesh = intersects.filter((element) => this.projectData.main.pcr[element.object.name]);
          const result = filterMesh.filter((element) => this.projectData.main.pcr[element.object.name].level == this.level_selected);
          if (result[0]) {
            result[0].object.material.color.set(0xd7da5a);
            result[0].object.material.opacity = 0.7;
          }
        } else {
          allChilds.forEach((element) => {
            if (element.type == "Mesh") {
              element.material.color.set(0xc5c5c2);
              element.material.opacity = 0.3;
            }
          });
        }
      }
    });
  },
};
</script>

<style lang="scss">
.render {
  &_wrapper {
    position: relative;
    width: 99vw;
    height: 99vh;
  }
}

.editor {
  position: absolute;
  top: 5px;
  left: 5px;
}


.exit {
  position: absolute;
  top: 5px;
  right: 5px;
}


.mainWindow {
  overflow: hidden;
  user-select: none;
  padding: 0;
  margin: 0;
}

.render-wrapper {
  height: 99%;
  width: 99%;
}

.three_detoggle {
  position: absolute;
  top: 50px;
  left: 50px;
  width: 100px;
  height: 50px;
  background-color: #c5c5c2;
}

.testcolor {
  color: #d80202;
}
</style>
