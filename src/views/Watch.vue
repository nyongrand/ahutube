<template>
  <div id="watch">
    <v-container fluid>
      <v-row>
        <v-col cols="11" class="mx-auto">
          <v-row>
            <v-col cols="12" sm="12" md="8" lg="8">
              <v-skeleton-loader
                type="card-avatar, article, actions"
                :loading="videoLoading"
                tile
                large
              >
                <v-responsive>
                  <video
                    controls
                    :poster="`/data/thumbnail/${video.thumbnail}`"
                    autoplay="muted"
                  >
                    <source :src="videoUrl" type="video/mp4" />
                  </video>
                </v-responsive>
                <v-card flat tile class="card">
                  <v-card-title class="pl-0 pb-0">{{
                    video.title
                  }}</v-card-title>
                  <div id="btns" class="d-flex flex-wrap justify-space-between">
                    <v-card-subtitle
                      class="pl-0 pt-0 pb-0 subtitle-1"
                      style="line-height: 2.4em;"
                    >
                      {{ video.viewCount.toLocaleString() }} views<v-icon
                        >mdi-circle-small</v-icon
                      >{{ video.createdAt }}
                    </v-card-subtitle>
                    <v-card-actions class="pt-0 pl-0 grey--text">
                      <v-btn text
                        ><v-icon class="pr-2">mdi-thumb-up</v-icon>
                        {{ video.likes.toLowerCase() }}</v-btn
                      >
                      <v-btn text
                        ><v-icon class="pr-2">mdi-thumb-down</v-icon>
                        {{ video.dislikes.toLowerCase() }}</v-btn
                      >
                      <v-btn text><v-icon>mdi-share</v-icon> Share</v-btn>
                      <v-btn text
                        ><v-icon>mdi-playlist-plus</v-icon> Save</v-btn
                      >
                    </v-card-actions>
                  </div>
                </v-card>

                <v-row class="justify-space-between">
                  <v-col cols="6" sm="6" md="5" lg="5">
                    <v-card class="transparent" flat>
                      <v-list-item three-line>
                        <v-list-item-avatar size="50"
                          ><v-img
                            :src="`/data/avatar/${channel.id}.jpg`"
                          ></v-img
                        ></v-list-item-avatar>
                        <v-list-item-content class="align-self-auto">
                          <v-list-item-title class="font-weight-medium mb-1">{{
                            channel.name
                          }}</v-list-item-title>
                          <v-list-item-subtitle
                            >{{ video.subscribers }} subscribers
                          </v-list-item-subtitle>
                        </v-list-item-content>
                      </v-list-item>
                    </v-card>
                  </v-col>
                  <v-col cols="6" sm="6" md="4" lg="4">
                    <div class="d-flex justify-end align-center">
                      <v-btn class="red white--text mt-6" tile depressed
                        >Subscribed</v-btn
                      >
                      <v-btn icon class="ml-5 mt-6"
                        ><v-icon>mdi-bell</v-icon></v-btn
                      >
                    </div>
                  </v-col>
                  <v-col class="pl-11" offset="1" cols="11" md="11">
                    <p>
                      {{
                        truncate
                          ? truncateText(video.description, 150)
                          : video.description
                      }}
                    </p>
                    <v-btn text class="remove-hover-bg" @click="show"
                      >Show More</v-btn
                    >
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="auto" class="mx-auto body-1">
                    <p>Comments are turned off.</p>
                  </v-col>
                </v-row>
              </v-skeleton-loader>
            </v-col>

            <v-col cols="12" sm="12" md="4" lg="4">
              <hr class="grey--text" />
              <h4 class="mb-3 mt-3">Up next</h4>
              <div v-for="(v, i) in recomendations" :key="i" class="mb-5">
                <v-skeleton-loader
                  class="mx-auto"
                  type="list-item-avatar-three-line"
                  :loading="loading"
                  tile
                  large
                >
                  <v-card class="card" tile flat :to="`watch?v=` + v.id">
                    <v-row no-gutters>
                      <v-col class="mx-auto" cols="3" sm="3" md="5" lg="5">
                        <!-- <v-responsive max-height="100%"> -->
                        <v-img
                          class="align-center"
                          :src="`/data/thumbnail/${v.thumbnail}`"
                        >
                        </v-img>
                        <!-- </v-responsive> -->
                      </v-col>
                      <v-col>
                        <div class="ml-2">
                          <v-card-title
                            class="pl-2 pt-0 subtitle-1 font-weight-bold"
                            style="line-height: 1"
                          >
                            {{ v.title }}
                          </v-card-title>

                          <v-card-subtitle
                            class="pl-2 pt-2 pb-0"
                            style="line-height: 1"
                          >
                            {{ v.uploader }}<br />
                            {{ v.views.toLowerCase() }}
                            views<v-icon>mdi-circle-small</v-icon>6 hours ago
                          </v-card-subtitle>
                        </div>
                      </v-col>
                    </v-row>
                  </v-card>
                </v-skeleton-loader>
              </div>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data: () => ({
    loading: true,
    videoLoading: true,
    video: {},
    channel: {},
    recomendations: [],
    truncate: true,
    comment: "",
    showCommentBtns: false,
    repliesInput: {},
  }),

  computed: {
    videoUrl() {
      return `http://localhost:5000/${this.channel.folder}/${this.video.filename}`;
    },
  },

  watch: {
    $route: "fetchData",
  },

  mounted() {
    this.fetchData();
  },

  methods: {
    async fetchData() {
      this.loading = true;
      this.videoLoading = true;

      this.video = await this.getVideo(this.$route.query.v);
      this.channel = await this.getChannel(this.video.uploaderId);
      this.recomendations = await this.getVideoRecomendations();

      this.loading = false;
      this.videoLoading = false;
    },
    async getVideo(id) {
      const res = await axios.get(`/data/videos/${id}.json`);
      return res.data;
    },
    async getChannel(id) {
      const res = await axios.get(`/data/channels/${id}.json`);
      return res.data;
    },
    async getVideoRecomendations() {
      // load channels list
      const res1 = await axios.get(`/data/channels.json`);
      const channels = res1.data;

      // get all video list
      const res2 = await Promise.all(
        channels.map((x) => axios.get(`/data/channels/${x.id}/videos.json`))
      );

      // get random video recomendations
      const recomendations = [];
      for (let i = 0; i <= 10; i++) {
        const c = this.getRandomInt(channels.length);
        const channelVideos = res2[c].data;

        const v = this.getRandomInt(channelVideos.length);
        recomendations.push(channelVideos[v]);
      }

      return recomendations;
    },
    showReply(id) {
      this.$refs[id][0].classList.toggle("d-none");
    },
    hideReply(id) {
      this.$refs[`form${id}`][0].reset();
      this.$refs["reply" + id][0].classList.toggle("d-none");
    },
    addReply(id) {
      this.$refs[`form${id}`][0].reset();
      console.log(this.$refs[`input${id}`][0].$refs.input.value);
    },
    show(event) {
      if (event.target.innerText === "SHOW MORE") {
        this.truncate = false;
        event.target.innerText = "SHOW LESS";
      } else {
        this.truncate = true;
        event.target.innerText = "SHOW MORE";
      }
    },
    truncateText(string = "", num) {
      if (string.length <= num) {
        return string;
      }
      return string.slice(0, num);
    },
    getRandomInt(max) {
      max = Math.floor(max);
      return Math.floor(Math.random() * max);
    },
  },
};
</script>

<style lang="scss">
video {
  max-width: 100%;
  /* min-height: 360px; */
  /* width: 640px;
  height: 360px; */
}

#btns {
  border-bottom: 1px solid #e0d8d8;
  button {
    color: #7f7f7f;
  }
}

button.v-btn.remove-hover-bg {
  background-color: initial !important;
  &:hover {
    background-color: #f9f9f9;
  }
}
</style>
