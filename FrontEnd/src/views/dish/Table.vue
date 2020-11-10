<template>
  <!-- Table Component -->
  <b-card>
    <div slot="header" v-html="caption"></div>
    <b-table :dark="false" :hover="hover" :striped="striped" :bordered="bordered" :small="small" :fixed="false" responsive="sm" :items="items" :fields="captions" :current-page="currentPage" :per-page="perPage" @row-clicked="rowClicked">
      <!-- Customizing the Name column -->
      <template slot="dishName" slot-scope="data">
        <b-row class="align-items-center mt-1">
          <a  class="ml-2" href="#"v-on:click="showRow(data.item)" >{{data.item.dishName}}</a>
          <b-button  class="ml-2" variant="success" v-on:click="editRow(data.item.dishId)">
            <i class="fa fa-pencil"></i>&nbsp;
          </b-button>
          <b-button class="ml-2" variant="danger" v-on:click="deleteRow(data.item)">
            <i class="fa fa-trash-o"></i>&nbsp;
          </b-button>
        </b-row>
      </template>

      <!-- Customizing rating column -->
      <template slot="dishTotalRating" slot-scope="data">
        <b-row class="align-items-center mt-1 ml-2">
          {{data.item.dishTotalRating}}
          <b-button  class="ml-2" variant="primary" v-on:click="viewRating(data.item.dishId)">
            <i class="fa fa-eye"></i>&nbsp;
          </b-button>

        </b-row>


      </template>

    </b-table>
    <nav>
      <b-pagination :total-rows="totalRows" :per-page="perPage" v-model="currentPage" prev-text="Prev" next-text="Next" hide-goto-end-buttons/>
    </nav>

    <b-modal
      title="Delete Dish"
      variant="danger"
      header-bg-variant="danger"
      content-class="border-danger"
      v-model="dangerModal"
      @ok="deleteDish"
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

      {key: 'dishName', sortable: true},
      {key: 'dishDescription', label: 'Description'},
      {key: 'dishPrice', sortable: true},
      {key: 'dishTotalRating', sortable: true}
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
    editRow(id){
      console.log(id);
      this.$router.push({name: 'EditDish', query: { dishId: id } });
    },
    //Show Delete popup
    deleteRow(item){
      console.log(item);
      this.deleteText = "Are you sure to delete Dish item \"" +  item.dishName + "\" with Dish Id: " + item.dishId +" ?";
      this.deleteId = item.dishId;
      this.dangerModal = true
    },
    //Send delete request to  API
    deleteDish(){
      this.dangerModal = false;
      this.$store.dispatch('deleteDish', this.deleteId).then(res => {
        console.log(res);
        this.reload()
      }).catch(err=>{
        console.log(err.message)

      })
    },
    viewRating(id){
      console.log(id);
      this.$router.push({name: 'RatingList', query: { dishId: id } });
    },
    showRow(item){
      console.log(item);
      this.$router.push({name: 'DishView', query: { dishId: item.dishId } });
    }
  }
}
</script>
