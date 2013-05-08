<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>

  <xsl:template match="custaccountupdcreditamt_mag">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          {lbl:custID}
        </td>
        <td class="colW2">
          <xsl:value-of select="@custid" />
        </td>
        <td class="colW1">
          {lbl:creditAmt}
        </td>
        <td class="colW2">
          <xsl:value-of select="@creditamt" />
        </td>
        <td class="colW1">
          {lbl:magID}
        </td>
        <td class="colW2">
          <xsl:value-of select="@magid" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          {lbl:userID}
        </td>
        <td class="colW2">
          <xsl:value-of select="@userid" />
        </td>
        <td class="colW1">
          {lbl:creditProblem}
        </td>
        <td class="colW2">
          <xsl:value-of select="@creditproblem" />
        </td>
        <td class="colW1">
          {lbl:language}
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>

      <tr>
        <td class="colW1">
          {lbl:subAccID}
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccid" />
        </td>
        <td colspan="4"></td>
      </tr>

    </table>
  </xsl:template>
</xsl:stylesheet>
