<template>
  <!-- Rating Table -->
  <b-card>
    <div slot="header" v-html="caption"></div>
    <b-table :dark="false" :hover="hover" :striped="striped" :bordered="bordered" :small="small" :fixed="false" responsive="sm" :items="items" :fields="captions" :current-page="currentPage" :per-page="perPage" @row-clicked="rowClicked">
      <template slot="actions" slot-scope="data">
        <b-row class="align-items-center mt-1 ml-2">
          <b-button class="align-items-center" variant="danger" v-on:click="deleteRow(data.item)">
            <i class="fa fa-trash-o"></i>&nbsp;
            Delete
          </b-button>
        </b-row>
      </template>

    </b-table>
    <nav>
      <b-pagination :total-rows="totalRows" :per-page="perPage" v-model="currentPage" prev-text="Prev" next-text="Next" hide-goto-end-buttons/>
    </nav>


    <b-modal
      title="Delete Rating"
      variant="danger"
      header-bg-variant="danger"
      content-class="border-danger"
      v-model="dangerModal"
      @ok="deleteRating"
      ok-variant="danger"
    >
      {{deleteText}}
    </b-modal>
  </b-card>


</template>

<script>


export default {
  name: 'c-table',
  inheritAttrs: false,
  props: {
    caption: {
      type: String,
      default: 'Table'
    },
    hover: {
      type: Boolean,
      default: false
    },
    striped: {
      type: Boolean,
      default: false
    },
    bordered: {
      type: Boolean,
      default: false
    },
    small: {
      type: Boolean,
      default: false
    },
    fixed: {
      type: Boolean,
      default: false
    },
    tableData: {
      type: [Array, Function],
      default: () => []
    },
    fields: {
      type: [Array, Object],
      default: () => []
    },
    perPage: {
      type: Number,
      default: 10
    },
    dark: {
      type: Boolean,
      default: false
    },
    reload:{
      type: Function ,

    }
  },
  data: () => {
    return {
      currentPage: 1,
      dangerModal: false,
      deleteText: "",
      deleteId: ""
    }
  },
  computed: {
    items: function() {
      const items =  this.tableData;
      return Array.isArray(items) ? items : items()
    },
    totalRows: function () { return this.getRowCount() },
    captions: function() { return [

      {key: 'userName', sortable: true},
      {key: 'userEmail',sortable: true},
      {key: 'rate', sortable: true},
      {key: 'review', sortable: true},
      {key: 'actions'}
    ] }
  },
  methods: {
    getBadge (status) {
      return status === 'Active' ? 'success'
        : status === 'Inactive' ? 'secondary'
          : status === 'Pending' ? 'warning'
            : status === 'Banned' ? 'danger' : 'primary'
    },
    getRowCount: function () {
      return this.items.length
    },
    rowClicked (item) {
      this.$emit('row-clicked', item);
      console.log(item)
    },
    showRow(id){
      console.log(id)
    },
    editRow(id){
      console.log(id)
    },
    //Show delete popup
    deleteRow(item){
      console.log(item);
      this.deleteText = "Are you sure to delete this Review  by \"" +  item.userName + "\" with Rating of : " + item.rate +"/5 ?";
      this.deleteId = item.ratingId;
      this.dangerModal = true
    },
    //Delete request to API
    deleteRating(){
      this.dangerModal = false;
      this.$store.dispatch('deleteRating', this.deleteId).then(res => {
        console.log(res);
        this.reload()
      }).catch(err=>{
        console.log(err.message)

      })
    },
  }
}
</script>
