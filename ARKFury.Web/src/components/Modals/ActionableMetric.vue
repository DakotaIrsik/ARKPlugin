<template>
  <modal :show.sync="modals.actionableMetric" headerClasses="justify-content-center">
    <h4 slot="header" class="title title-up">Sorry - This feature doesn't currently exist</h4>
  <div class="modal-body">
    <p>How important is this feature?</p>
    <b-button-toolbar key-nav aria-label="Toolbar with button groups">
      <b-button class="form-control" v-model="importance" >
        Not very
      </b-button>
      <b-button class="form-control" v-model="importance">
        Nice to have
      </b-button>
      <b-button class="form-control" v-model="importance">
        Required
      </b-button>
    </b-button-toolbar>
    <div v-if="importance != 'Not Very'">
      <p>Would you be willing to pay for this stand-alone feature?</p>
      <b-button-toolbar key-nav aria-label="Toolbar with button groups">
        <b-button class="form-control" v-model="willingToPayForFeature">
          No
        </b-button>
        <b-button class="form-control" v-model="willingToPayForFeature">
          Yes
        </b-button>
      </b-button-toolbar>
      <div v-if="willingToPayForFeature === 'Yes'">
        <p>How much?</p>
        <b-button-toolbar key-nav aria-label="Toolbar with button groups">
          <b-button class="form-control" v-model="willingToPayForFeatureAmount">
            $1
          </b-button>
          <b-button class="form-control" v-model="willingToPayForFeatureAmount">
            $2
          </b-button>
          <b-button class="form-control" v-model="willingToPayForFeatureAmount">
            $5
          </b-button>
          <b-button class="form-control" v-model="willingToPayForFeatureAmount">
            Not sure
          </b-button>
        </b-button-toolbar>
      </div>
      <p>Would you be willing to pay a monthly subscription if this and other features were included?</p>
      <div class="btn-group">
        <b-button class="form-control" v-model="willingToPayForSubscription">
          No
        </b-button>
        <b-button class="form-control" v-model="willingToPayForSubscription">
          Yes
        </b-button>
      </div>
      <div v-if="willingToPayForSubscription === 'Yes'">
        <p>How much would you be willing to pay monthly?</p>
        <b-button-toolbar key-nav aria-label="Toolbar with button groups">
          <b-button class="form-control" v-model="willingToPayForSubscriptionAmount">
            $1
          </b-button>
          <b-button class="form-control" v-model="willingToPayForSubscriptionAmount">
            $5
          </b-button>
          <b-button class="form-control" v-model="willingToPayForSubscriptionAmount">
            $15
          </b-button>
          <b-button class="form-control" v-model="willingToPayForSubscriptionAmount">
            Not sure
          </b-button>
        </b-button-toolbar>
      </div>
    </div>
    <div class="md-form">
        <textarea type="text" id="feedback" v-model="comments" class="md-textarea form-control" mdbInput></textarea>
        <label for="feedback">Please give us any additional feeback.</label>
      </div>
  <div class="modal-footer">
    <button type="button" mdbBtn color="secondary" class="waves-light" aria-label="Close" (click)="modalRef.hide()" >Close</button>
    <button type="button" mdbBtn color="primary" class="relative waves-light" (click)="submitFeedback()">Submit Feedback</button>
  </div>
</div>
    <br />
    <br />

  </modal>
</template>
<script>
</script>
<style scoped>
</style>
